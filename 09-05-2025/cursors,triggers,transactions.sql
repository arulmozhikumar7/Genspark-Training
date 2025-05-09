-- +----------------------+------------------------+---------------------------------------------+
-- | Type                 | Supported in PostgreSQL | Method                                      |
-- +----------------------+------------------------+---------------------------------------------+
-- | DML Logging          | ✅ Yes                 | Triggers (AFTER INSERT, AFTER UPDATE, etc.) |
-- | DDL Logging          | ✅ Yes (partial)       | Event Triggers (ON ddl_command_end)         |
-- | Logon/Logoff Logging | ❌ No triggers, but ✅ possible | Enable server logs (log_connections),      |
-- |                      |                        | query pg_stat_activity                      |
-- +----------------------+------------------------+---------------------------------------------+

-- Logging and user connection tracking in PostgreSQL:
--  METHOD 1 : Using log_connections Setting (Log Connections)
-- 1.Open your postgresql.conf file.
-- 2.Set the following parameter:
-- 3.log_connections = on
-- 4.Restart PostgreSQL for the changes to take effect.
-- PostgreSQL's event triggers cannot track user sessions or logins.
-- METHOD 2 : Using pg_stat_activity View (Monitor Active Sessions)
-- You can query the pg_stat_activity system view to see active connections and track sessions.
-- SELECT pid, username, application_name, client_addr, backend_start
-- FROM pg_stat_activity
-- WHERE state = 'active';
 
 

--CURSOR QUESTIONS

--1.Write a cursor that loops through all films and prints titles longer than 120 minutes.
DO $$
DECLARE
    film_rec RECORD;
    film_cursor CURSOR FOR
        SELECT title, length FROM film;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO film_rec;
        EXIT WHEN NOT FOUND;

        IF film_rec.length > 120 THEN
            RAISE NOTICE 'Title: %, Length: %', film_rec.title, film_rec.length;
        END IF;
    END LOOP;
    CLOSE film_cursor;
END $$;

--2.Create a cursor that iterates through all customers and counts how many rentals each made.
DO $$
DECLARE
    cust_rec RECORD;
    rental_count INT;
    cust_cursor CURSOR FOR
        SELECT customer_id, first_name, last_name FROM customer;
BEGIN
    OPEN cust_cursor;
    LOOP
        FETCH cust_cursor INTO cust_rec;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(*) INTO rental_count
        FROM rental
        WHERE customer_id = cust_rec.customer_id;

        RAISE NOTICE 'Customer: %, Rentals: %',
            cust_rec.first_name || ' ' || cust_rec.last_name, rental_count;
    END LOOP;
    CLOSE cust_cursor;
END $$;

--3.Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

DO $$
DECLARE
    film_rec RECORD;
    rental_count INT;
    film_cursor CURSOR FOR
        SELECT film_id, title FROM film;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO film_rec;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(*) INTO rental_count
        FROM rental r
        JOIN inventory i ON r.inventory_id = i.inventory_id
        WHERE i.film_id = film_rec.film_id;

        IF rental_count < 5 THEN
            UPDATE film
            SET rental_rate = rental_rate + 1
            WHERE film_id = film_rec.film_id;

            RAISE NOTICE 'Updated % (ID: %) - Rentals: %', film_rec.title, film_rec.film_id, rental_count;
        END IF;
    END LOOP;
    CLOSE film_cursor;
END $$;

--4.Create a function using a cursor that collects titles of all films from a particular category.

CREATE OR REPLACE FUNCTION get_film_titles_by_category(cat_name TEXT)
RETURNS TABLE(title TEXT) AS $$
DECLARE
    film_cursor CURSOR FOR
        SELECT f.title
        FROM film f
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        WHERE c.name = cat_name;

    film_rec RECORD;
BEGIN
    OPEN film_cursor;

    LOOP
        FETCH film_cursor INTO film_rec;
        EXIT WHEN NOT FOUND;
        title := film_rec.title;
        RETURN NEXT;
    END LOOP;

    CLOSE film_cursor;
END;
$$ LANGUAGE plpgsql;


SELECT get_film_titles_by_category('Action');

--5.Loop through all stores and count how many distinct films are available in each store using a cursor.

DO
$$
DECLARE
    store_row RECORD;
    film_count INTEGER;
    store_cursor CURSOR FOR
        SELECT store_id FROM store;

BEGIN
    OPEN store_cursor;

    LOOP
        FETCH store_cursor INTO store_row;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(DISTINCT film_id)
        INTO film_count
        FROM inventory
        WHERE store_id = store_row.store_id;

        -- Output 
        RAISE NOTICE 'Store ID: %, Distinct Films: %', store_row.store_id, film_count;
    END LOOP;


    CLOSE store_cursor;
END
$$ LANGUAGE plpgsql;



--TRIGGER QUESTIONS	

--1. Write a trigger that logs whenever a new customer is inserted.

CREATE TABLE customer_insert_log (
    log_id SERIAL PRIMARY KEY,
    customer_id INT,
    full_name VARCHAR(255),
    email VARCHAR(255),
    insert_timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_new_customer_insert() 
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO customer_insert_log (customer_id, full_name, email, insert_timestamp)
    VALUES (NEW.customer_id, NEW.first_name || ' ' || NEW.last_name, NEW.email, CURRENT_TIMESTAMP);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER customer_insert_trigger
AFTER INSERT ON customer
FOR EACH ROW
EXECUTE FUNCTION log_new_customer_insert();

INSERT INTO customer (first_name, last_name, email,store_id,address_id)
VALUES ('John', 'Doe', 'john.doe@example.com',1,1);

SELECT * FROM  customer_insert_log 

--2. Create a trigger that prevents inserting a payment of amount 0.

CREATE OR REPLACE FUNCTION prevent_zero_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.amount = 0 THEN
        RAISE EXCEPTION 'Payment amount cannot be zero.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER prevent_zero_payment_trigger
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_zero_payment();

INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 1, 1, 5.99, CURRENT_TIMESTAMP);
-- Success


INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 1, 1, 0, CURRENT_TIMESTAMP);
-- ERROR: Payment amount cannot be zero.

--3.Set up a trigger to automatically set last_update on the film table before update.

CREATE OR REPLACE FUNCTION update_last_update_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.last_update := CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_last_update_before_update
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_last_update_column();

UPDATE film
SET title = 'New Film Title'
WHERE film_id = 1;

SELECT title, last_update
FROM film
WHERE film_id = 1;

--4.Create a trigger to log changes in the inventory table (insert/delete).

CREATE OR REPLACE FUNCTION log_inventory_activity()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        RAISE NOTICE 'Inventory INSERT: inventory_id=%, film_id=%, store_id=%',
            NEW.inventory_id, NEW.film_id, NEW.store_id;
    ELSIF TG_OP = 'DELETE' THEN
        RAISE NOTICE 'Inventory DELETE: inventory_id=%, film_id=%, store_id=%',
            OLD.inventory_id, OLD.film_id, OLD.store_id;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_inventory_activity
AFTER INSERT OR DELETE ON inventory
FOR EACH ROW
EXECUTE FUNCTION log_inventory_activity();

INSERT INTO inventory (film_id, store_id) VALUES (1, 1);

--5. Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

CREATE OR REPLACE FUNCTION prevent_rental_if_owe_more_than_50()
RETURNS TRIGGER AS $$
DECLARE
    rental_rate NUMERIC;
    total_owed NUMERIC;
BEGIN
    -- Get the rental rate for the film from the inventory
    SELECT f.rental_rate
    INTO rental_rate
    FROM inventory i
    JOIN film f ON i.film_id = f.film_id
    WHERE i.inventory_id = NEW.inventory_id;

    -- Check if the customer has paid for this rental
    SELECT SUM(p.amount)
    INTO total_owed
    FROM payment p
    WHERE p.customer_id = NEW.customer_id
      AND p.rental_id = NEW.rental_id;

    -- If the customer owes more than $50 (or doesn't have a payment), raise an exception
    IF total_owed IS NULL OR total_owed < rental_rate THEN
        RAISE EXCEPTION 'Customer % owes more than $50 and cannot make a rental', NEW.customer_id;
    END IF;

    -- Proceed with the rental if they don’t owe more than $50
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Create the trigger to check before insert on rental
CREATE TRIGGER check_customer_balance_before_rental
BEFORE INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION prevent_rental_if_owe_more_than_50();

-- Example where customer owes more than $50 and cannot rent
INSERT INTO rental (customer_id, inventory_id, rental_date, return_date)
VALUES (1, 10, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '2 days');


--Transaction-Based Questions

--1. Write a transaction that inserts a customer and an initial rental in one atomic operation.

BEGIN;

WITH new_customer AS (
  INSERT INTO customer (first_name, last_name, email, address_id, store_id, create_date)
  VALUES ('John', 'Doe', 'john.doe@example.com', 1, 1, CURRENT_TIMESTAMP)
  RETURNING customer_id
)
INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
SELECT CURRENT_TIMESTAMP, 1, customer_id, 1 FROM new_customer;

COMMIT;

--2.Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

BEGIN;

UPDATE film
SET title = 'The Fake Sequel'
WHERE film_id = 1;

-- Insert into inventory with an invalid FILM_id to trigger failure
-- film_id = 0 is assumed invalid
INSERT INTO inventory (film_id, store_id, last_update)
VALUES (0, 9999, CURRENT_TIMESTAMP);

-- If both succeed, commit
COMMIT;

ROLLBACK;

--3.Create a transaction that transfers an inventory item from one store to another.
BEGIN;

UPDATE inventory
SET store_id = 2,
    last_update = CURRENT_TIMESTAMP
WHERE inventory_id = 1;

COMMIT;

--4.Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

BEGIN;

-- Update first payment record
UPDATE payment
SET amount = amount + 5
WHERE payment_id = 1;

-- Set a SAVEPOINT before the next update
SAVEPOINT before_second_update;

-- Update second payment record
UPDATE payment
SET amount = amount + 10
WHERE payment_id = 2;

-- Decide to undo that second update
ROLLBACK TO SAVEPOINT before_second_update;

-- Continue with a third update
UPDATE payment
SET amount = amount + 20
WHERE payment_id = 3;

-- Commit the successful changes
COMMIT;


