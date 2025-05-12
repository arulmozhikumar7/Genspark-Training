
/* 
  1️⃣ Question:
  In a transaction, if I perform multiple updates and an error happens in the third statement,
  but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
  Will my first two updates persist?

  Answer:
  NO. If no SAVEPOINT is used, a ROLLBACK will undo the entire transaction.
  All updates made before the error will also be rolled back.
*/
BEGIN;
UPDATE employees SET salary = salary + 1000 WHERE id = 1; -- 1st update
UPDATE employees SET salary = salary + 2000 WHERE id = 2; -- 2nd update
-- Simulate error on 3rd update:
UPDATE employees SET salary = 'invalid' WHERE id = 3;     -- causes error
ROLLBACK;  -- This undoes all previous changes in this transaction


/*
  2️⃣ Question:
  Suppose Transaction A updates Alice’s balance but does not commit.
  Can Transaction B read the new balance if the isolation level is READ COMMITTED?

  Answer:
  NO. READ COMMITTED prevents reading uncommitted data.
  Transaction B will only see committed values at the time its query runs.
*/

/*
  3️⃣ Question:
  What happens if two concurrent transactions execute:
  UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';

  Answer:
  PostgreSQL uses row-level locks.
  The first transaction locks the row.
  The second one waits for the first to commit or rollback before proceeding.
  No data is overwritten silently.
*/


/*
  4️⃣ Question:
  If I issue ROLLBACK TO SAVEPOINT after_alice;, will it undo only changes after the savepoint or everything?

  Answer:
  It undoes only the changes made after the SAVEPOINT.
  Changes made before the savepoint remain intact in the transaction.
*/

BEGIN;
SAVEPOINT after_alice;
UPDATE accounts SET balance = balance - 100 WHERE name = 'Alice';
UPDATE accounts SET balance = balance - 50 WHERE name = 'Bob';
ROLLBACK TO SAVEPOINT after_alice; -- only Bob's change is undone
COMMIT;


/*
  5️Question:
  Which isolation level in PostgreSQL prevents phantom reads?

  Answer:
  SERIALIZABLE is the only level that guarantees prevention of phantom reads.
*/

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;


/*
  6️⃣ Question:
  Can PostgreSQL perform dirty reads (i.e., read uncommitted data from another transaction)?

  Answer:
  NO. PostgreSQL does not support dirty reads at any isolation level.
  The lowest level, READ COMMITTED, still prevents dirty reads.
*/


/*
  7️⃣ Question:
  If autocommit is ON (default in Postgres), and I execute an UPDATE,
  is it safe to assume the change is immediately committed?

  Answer:
  YES. With autocommit ON, each statement runs in its own transaction
  and is committed automatically if it completes successfully.
*/

-- Example with autocommit ON (default behavior)
UPDATE products SET price = price + 10 WHERE category = 'Electronics';
-- This change is automatically committed

/*
  8️⃣ Question:
  If I do this:

    BEGIN;
    UPDATE accounts SET balance = balance - 500 WHERE id = 1;
    -- (No COMMIT yet)

  And from another session, I run:

    SELECT balance FROM accounts WHERE id = 1;

  Will the second session see the deducted balance?

  Answer:
  NO. The second session will NOT see the deducted balance because the first transaction has not committed.
  PostgreSQL uses MVCC (Multi-Version Concurrency Control), which ensures that uncommitted changes are
  invisible to other sessions, even under READ COMMITTED isolation level (the default).
*/

-- Session 1:
BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- No COMMIT yet

-- Session 2 (in parallel):
-- This will see the original balance (before deduction) until Session 1 commits.
SELECT balance FROM accounts WHERE id = 1;

