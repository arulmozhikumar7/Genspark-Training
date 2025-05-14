#### The goal of this process is to set up streaming replication between a primary PostgreSQL server and a standby server, ensuring real-time data synchronization for high availability and failover.

### TO SETUP STREAMING REPLICATION

#### STEP 1:

Create directories:

```bash
mkdir C:\pgdata\pgdata_primary
mkdir C:\pgdata\pgdata_standby
```

#### STEP 2:

Initialize the primary cluster at `C:\pgdata\pgdata_primary`

```bash
"C:\Program Files\PostgreSQL\17\bin\initdb.exe" -D "C:\pgdata\pgdata_primary" -U postgres
```

Make the following changes in `postgresql.conf` to allow replication:

```conf
# CONNECTIONS
listen_addresses = 'localhost'
port = 5432

# REPLICATION
wal_level = replica
max_wal_senders = 10
wal_keep_size = 64
```

Add this to `pg_hba.conf` to authorize standby connections:

```conf
host    replication     all     127.0.0.1/32    trust
```

#### STEP 3:

Start the primary server on port 5432:

```bash
"C:\Program Files\PostgreSQL\17\bin\pg_ctl.exe" -D "C:\pgdata\pgdata_primary" -o "-p 5432" start
```

#### STEP 4:

Clone the primary into the standby directory using `pg_basebackup`:

```bash
"C:\Program Files\PostgreSQL\17\bin\pg_basebackup.exe" ^
  -D "C:\pgdata\pgdata_standby" ^
  -R ^
  -X stream ^
  -C ^
  -S replica_slot ^
  -h localhost ^
  -p 5432 ^
  -U postgres ^
  --write-recovery-conf
```

#### STEP 5:

Start the standby server on port 5433:

```bash
"C:\Program Files\PostgreSQL\17\bin\pg_ctl.exe" -D "C:\pgdata\pgdata_standby" -o "-p 5433" start
```
