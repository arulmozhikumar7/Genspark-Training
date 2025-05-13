--Task 1: Try two concurrent updates to same row → see lock in action.

-- Session 1
BEGIN;

-- This acquires a row-level exclusive lock on customer_id = 1
UPDATE customer
SET first_name = 'John'
WHERE customer_id = 1;

-- Do NOT COMMIT yet; hold the transaction
-- (this keeps the lock on the row)

COMMIT;

-- Session 2
BEGIN;

UPDATE customer
SET last_name = 'Doe'
WHERE customer_id = 1;

COMMIT;

SELECT first_name , last_name from customer where customer_id = 1;

--Task 2: Write a query using SELECT...FOR UPDATE and check how it locks row.

-- Session 1
BEGIN;

-- This locks the row with customer_id = 1
SELECT *
FROM customer
WHERE customer_id = 1
FOR UPDATE;

-- Row is now locked; no one else can update/delete it until COMMIT or ROLLBACK

COMMIT;

-- Session 2
BEGIN;

-- This will hang until Session 1 releases the lock
UPDATE customer
SET last_name = 'Blocked'
WHERE customer_id = 1;

-- Optional: COMMIT after it unblocks
COMMIT ;

--Task 3: Intentionally create a deadlock and observe PostgreSQL cancel one transaction.

CREATE TABLE accounts (
    account_id SERIAL PRIMARY KEY,
    balance DECIMAL,
    owner_name VARCHAR(100)
);

INSERT INTO accounts (balance, owner_name) VALUES (1000.00, 'Alice');
INSERT INTO accounts (balance, owner_name) VALUES (1500.00, 'Bob');

--Session 1
BEGIN;
-- Lock Alice's account row
UPDATE accounts SET balance = balance + 100 WHERE account_id = 1;

--Session 2
BEGIN;
-- Lock Bob's account row
UPDATE accounts SET balance = balance - 100 WHERE account_id = 2;

--Session 1
-- Try to lock Bob's account, this will cause deadlock once Session 2 locks Alice's account
UPDATE accounts SET balance = balance - 100 WHERE account_id = 2;

--Session 2
-- Try to lock Alice's account, this will cause deadlock once Session 1 locks Bob's account
UPDATE accounts SET balance = balance + 100 WHERE account_id = 1;

/* Output :
	ERROR:  deadlock detected
Process 1408 waits for ShareLock on transaction 1079; blocked by process 13648.
Process 13648 waits for ShareLock on transaction 1080; blocked by process 1408. 

SQL state: 40P01
Detail: Process 1408 waits for ShareLock on transaction 1079; blocked by process 13648.
Process 13648 waits for ShareLock on transaction 1080; blocked by process 1408.
Hint: See server log for query details.
Context: while updating tuple (0,1) in relation "accounts"
*/


--Task 4. Use pg_locks query to monitor active locks.
SELECT pid, usename, state, query, wait_event_type, wait_event
FROM pg_stat_activity
WHERE state = 'active';

SELECT 
    pid, 
    relation::regclass AS locked_table, 
    mode, 
    granted 
FROM pg_locks 
WHERE granted = false;
ROLLBACK;
