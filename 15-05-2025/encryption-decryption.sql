/*
1. Create a stored procedure to encrypt a given text
Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
Use pgp_sym_encrypt(text, key) from pgcrypto.
2. Create a stored procedure to compare two encrypted texts
Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.
3. Create a stored procedure to partially mask a given text
Task: Write a procedure sp_mask_text that:
Shows only the first 2 and last 2 characters of the input string
Masks the rest with *
E.g., input: 'john.doe@example.com' → output: 'jo***************om'
4. Create a procedure to insert into customer with encrypted email and masked name
Task:
Call sp_encrypt_text for email
Call sp_mask_text for first_name
Insert masked and encrypted values into the customer table
Use any valid address_id and store_id to satisfy FK constraints.
5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
Task:
Write sp_read_customer_masked() that:
Loops through all rows
Decrypts email
Displays customer_id, masked first name, and decrypted email
*/

-- Enable pgcrypto
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Create Table
CREATE TABLE IF NOT EXISTS customer (
    customer_id SERIAL PRIMARY KEY,
    first_name TEXT,
    email BYTEA,
    address_id INT,
    store_id INT
);
--1. Create a stored procedure to encrypt a given text
CREATE OR REPLACE PROCEDURE sp_encrypt_text_proc(
    IN p_plain_text TEXT,
    IN p_key TEXT,
    OUT p_encrypted BYTEA
)
LANGUAGE plpgsql
AS $$
BEGIN
    p_encrypted := pgp_sym_encrypt(p_plain_text, p_key);
END;
$$;
-- Output : task 1
DO $$
DECLARE
    result BYTEA;
BEGIN
    CALL sp_encrypt_text_proc('Cristiano@example.com', 'secret123', result);
    RAISE NOTICE 'Encrypted: %', encode(result, 'hex');
END $$;

/*
NOTICE:  Encrypted: c30d04070302ec6f765a2eb33a6969d24601e9e3ba70dd68aa28f6a2a3aa829411395609f9cb09431bdf35ddc6e2a4fd5561c2b7a6d191fbf5b94225cc3684a376e0e31bda22e3c98e74d461075f06b822c3b4a9b348fa
DO 
*/

--2.Create a stored procedure to compare two encrypted texts
CREATE OR REPLACE PROCEDURE sp_compare_encrypted_proc(
    IN p_encrypted1 BYTEA,
    IN p_encrypted2 BYTEA,
    IN p_key TEXT,
    OUT p_result BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
    decrypted1 TEXT;
    decrypted2 TEXT;
BEGIN
    decrypted1 := pgp_sym_decrypt(p_encrypted1, p_key);
    decrypted2 := pgp_sym_decrypt(p_encrypted2, p_key);
    p_result := (decrypted1 = decrypted2);
END;
$$;
-- Output : task 2
DO $$
DECLARE
    encrypted1 BYTEA;
    encrypted2 BYTEA;
    is_equal BOOLEAN;
BEGIN
    -- Encrypt the same value twice (will give different ciphertexts due to salting)
    encrypted1 := pgp_sym_encrypt('john@example.com', 'secret123');
    encrypted2 := pgp_sym_encrypt('john@example.com', 'secret123');

    -- Call procedure to compare decrypted values
    CALL sp_compare_encrypted_proc(encrypted1, encrypted2, 'secret123', is_equal);
    
    -- Print the result
    RAISE NOTICE 'Are decrypted values equal? %', is_equal;
END $$;
/*
NOTICE:  Are decrypted values equal? t
DO
*/

--3. Create a stored procedure to partially mask a given text
CREATE OR REPLACE PROCEDURE sp_mask_text_proc(
    IN p_input TEXT,
    OUT p_masked TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    len INT;
BEGIN
    len := LENGTH(p_input);
    IF len <= 4 THEN
        p_masked := p_input;
    ELSE
        p_masked := SUBSTRING(p_input FROM 1 FOR 2) || REPEAT('*', len - 4) || SUBSTRING(p_input FROM len - 1 FOR 2);
    END IF;
END;
$$;
-- Output : task 3
DO $$
DECLARE
    task_3_output TEXT;
BEGIN
    CALL sp_mask_text_proc('Cristiano', task_3_output);
    RAISE NOTICE 'Masked Output: %', task_3_output;
END $$;

/*
NOTICE:  Masked Output: Cr*****no
DO
*/

--4. Create a procedure to insert into customer with encrypted email and masked name
CREATE OR REPLACE PROCEDURE sp_insert_customer(
    p_first_name TEXT,
    p_email TEXT,
    p_address_id INT,
    p_store_id INT,
    p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    masked_name TEXT;
    encrypted_email BYTEA;
BEGIN
    -- Call mask procedure
    CALL sp_mask_text_proc(p_first_name, masked_name);

    -- Call encrypt procedure
    CALL sp_encrypt_text_proc(p_email, p_key, encrypted_email);

    -- Insert into table
    INSERT INTO customer (first_name, email, address_id, store_id)
    VALUES (masked_name, encrypted_email, p_address_id, p_store_id);
END;
$$;
-- Output : task 4
CALL sp_insert_customer('Johnathan', 'john@example.com', 1, 1, 'secret123');
CALL sp_insert_customer('Cristiano', 'cr7@realmadrid.com', 1, 5, 'secret123');

SELECT * FROM CUSTOMER ;

--5. Create a procedure to fetch and display masked first_name and decrypted email for all customers

CREATE OR REPLACE PROCEDURE sp_decrypt_text_proc(
    IN p_encrypted BYTEA,
    IN p_key TEXT,
    OUT p_decrypted TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    p_decrypted := pgp_sym_decrypt(p_encrypted, p_key);
END;
$$;

CREATE OR REPLACE PROCEDURE sp_read_customer_masked(
    p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    r RECORD;
    decrypted_email TEXT;
BEGIN
    FOR r IN SELECT customer_id, first_name, email FROM customer LOOP
        CALL sp_decrypt_text_proc(r.email, p_key, decrypted_email);

        RAISE NOTICE 'ID: %, Name: %, Email: %', r.customer_id, r.first_name, decrypted_email;
    END LOOP;
END;
$$;
-- Output : task 5
CALL sp_read_customer_masked('secret123');

/*
NOTICE:  ID: 1, Name: Jo*****an, Email: john@example.com
NOTICE:  ID: 2, Name: Cr*****no, Email: cr7@realmadrid.com
CALL
*/