-- Phase 1 : Schema Design

/*
Tables (Normalized to 3NF):

1. **students**

   * `student_id (PK)`, `name`, `email`, `phone`

2. **courses**

   * `course_id (PK)`, `course_name`, `category`, `duration_days`

3. **trainers**

   * `trainer_id (PK)`, `trainer_name`, `expertise`

4. **enrollmentsnrollment**

   * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`

5. **certificates**

   * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`

6. **course\_trainers** (Many-to-Many if needed)

   * `course_id`, `trainer_id`
*/

-- Phase 2: DDL & DML

-- 1. students
CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone VARCHAR(15) UNIQUE
);

-- 2. trainers
CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise TEXT
);

-- 3. courses
CREATE TABLE courses (
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    category VARCHAR(50),
    duration_days INT CHECK (duration_days > 0),
    trainer_id INT REFERENCES trainers(trainer_id) ON DELETE SET NULL
);

-- 4. enrollments
CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT REFERENCES students(student_id) ON DELETE CASCADE,
    course_id INT REFERENCES courses(course_id) ON DELETE CASCADE,
    enroll_date DATE NOT NULL,
    UNIQUE(student_id, course_id)
);

-- 5. certificates
CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT UNIQUE REFERENCES enrollments(enrollment_id) ON DELETE CASCADE,
    issue_date DATE NOT NULL,
    serial_no VARCHAR(50) UNIQUE NOT NULL
);

-- Index on student_id in enrollments
CREATE INDEX idx_enrollments_student_id ON enrollments(student_id);

-- Index on email in students
CREATE INDEX idx_students_email ON students(email);

-- Index on course_id in enrollments
CREATE INDEX idx_enrollments_course_id ON enrollments(course_id);

-- 1. Insert Student
CREATE OR REPLACE PROCEDURE insert_student(
    student_name TEXT,
    student_email TEXT,
    student_phone TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO students(name, email, phone)
    VALUES (student_name, student_email, student_phone);
END;
$$;

-- 2. Insert Trainer
CREATE OR REPLACE PROCEDURE insert_trainer(
    trainer_name TEXT,
    expertise TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO trainers(trainer_name, expertise)
    VALUES (trainer_name, expertise);
END;
$$;


-- 3. Insert Course
CREATE OR REPLACE PROCEDURE insert_course(
    course_name TEXT,
    category TEXT,
    duration INT,
    trainer_id INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO courses(course_name, category, duration_days, trainer_id)
    VALUES (course_name, category, duration, trainer_id);
END;
$$;


-- 4. Insert Enrollment
CREATE OR REPLACE PROCEDURE insert_enrollment(
    student_id INT,
    course_id INT,
    enroll_date DATE
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO enrollments(student_id, course_id, enroll_date)
    VALUES (student_id, course_id, enroll_date);
END;
$$;


-- 5. Insert Certificate
CREATE OR REPLACE PROCEDURE insert_certificate(
    enrollment_id INT,
    issue_date DATE,
    serial_no TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO certificates(enrollment_id, issue_date, serial_no)
    VALUES (enrollment_id, issue_date, serial_no);
END;
$$;

-- Insert Trainers
CALL insert_trainer('Trainer 1', 'Java, Spring');
CALL insert_trainer('Trainer 2', 'React, Node');
CALL insert_trainer('Trainer 3', 'Python, ML');

-- Insert Students
CALL insert_student('Student 1', 'student1@example.com', '9000000001');
CALL insert_student('Student 2', 'student2@example.com', '9000000002');
CALL insert_student('Student 3', 'student3@example.com', '9000000003');
CALL insert_student('Student 4', 'student4@example.com', '9000000004');
CALL insert_student('Student 5', 'student5@example.com', '9000000005');

-- Insert Courses
CALL insert_course('Course 1', 'Programming', 30, 1);
CALL insert_course('Course 2', 'Web Dev', 45, 2);
CALL insert_course('Course 3', 'AI/ML', 60, 3);
CALL insert_course('Course 4', 'Prompt Engineering', 10, 3);
CALL insert_course('Course 5', 'System Design', 10, 3);

-- Insert Enrollments
CALL insert_enrollment(1, 1, '2025-01-15');
CALL insert_enrollment(1, 2, '2025-02-01');
CALL insert_enrollment(2, 2, '2025-02-10');
CALL insert_enrollment(3, 3, '2025-03-05');
CALL insert_enrollment(4, 4, '2025-03-05');
CALL insert_enrollment(5, 4, '2025-03-05');

-- Insert Certificates
CALL insert_certificate(1, '2025-03-01', 'CERT-0001');
CALL insert_certificate(2, '2025-03-15', 'CERT-0002');
CALL insert_certificate(4, '2025-04-01', 'CERT-0003');

-- Phase 3: SQL Joins Practice

-- List students and the courses they enrolled in
SELECT 
    s.student_id,
    s.name AS student_name,
    c.course_id,
    c.course_name
FROM 
    enrollments e
JOIN 
    students s ON e.student_id = s.student_id
JOIN 
    courses c ON e.course_id = c.course_id
ORDER BY 
    s.student_id;
-- Show students who received certificates with trainer names
SELECT 
    s.name AS student_name,
    c.course_name,
    t.trainer_name,
    cert.serial_no,
    cert.issue_date
FROM 
    certificates cert
JOIN 
    enrollments e ON cert.enrollment_id = e.enrollment_id
JOIN 
    students s ON e.student_id = s.student_id
JOIN 
    courses c ON e.course_id = c.course_id
JOIN 
    trainers t ON c.trainer_id = t.trainer_id
ORDER BY 
    cert.issue_date DESC;
-- Count number of students per course
SELECT 
    c.course_id,
    c.course_name,
    COUNT(e.student_id) AS total_students
FROM 
    courses c
LEFT JOIN 
    enrollments e ON c.course_id = e.course_id
GROUP BY 
    c.course_id, c.course_name
ORDER BY 
    total_students DESC;

-- Phase 4: Functions & Stored Procedures

/* Function:
-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates. */

CREATE OR REPLACE FUNCTION get_certified_students(p_course_id INT)
RETURNS TABLE (
    student_id INT,
    student_name VARCHAR(100),
    email VARCHAR(100),
    certificate_serial VARCHAR(50),
    issue_date DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.student_id,
        s.name,
        s.email,
        cert.serial_no,
        cert.issue_date
    FROM 
        certificates cert
    JOIN 
        enrollments e ON cert.enrollment_id = e.enrollment_id
    JOIN 
        students s ON e.student_id = s.student_id
    WHERE 
        e.course_id = p_course_id;
END;
$$ LANGUAGE plpgsql;


SELECT * FROM get_certified_students(1);

CREATE OR REPLACE PROCEDURE sp_enroll_student(
    p_student_id INT,
    p_course_id INT,
    p_completed BOOLEAN DEFAULT FALSE
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_enrollment_id INT;
    v_serial_no VARCHAR(50);
BEGIN
    -- Insert enrollment, if not exists (unique student_id + course_id)
    INSERT INTO enrollments(student_id, course_id, enroll_date)
    VALUES (p_student_id, p_course_id, CURRENT_DATE)
    ON CONFLICT (student_id, course_id) DO NOTHING;

    -- Get the enrollment_id
    SELECT enrollment_id INTO v_enrollment_id
    FROM enrollments
    WHERE student_id = p_student_id AND course_id = p_course_id;

    -- If completed flag is true, insert a certificate
    IF p_completed THEN
        -- Generate a simple serial_no 
        v_serial_no := 'CERT-' || v_enrollment_id || '-' || to_char(CURRENT_DATE, 'YYYYMMDD');

        -- Insert certificate only if it doesn't exist for this enrollment
        INSERT INTO certificates(enrollment_id, issue_date, serial_no)
        VALUES (v_enrollment_id, CURRENT_DATE, v_serial_no)
        ON CONFLICT (enrollment_id) DO NOTHING;
    END IF;
END;
$$;

CALL sp_enroll_student(1, 4, FALSE);
CALL sp_enroll_student(1, 3, TRUE);


--Phase 5: Cursor

-- Use a cursor to:
-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

CREATE OR REPLACE PROCEDURE print_students_without_certificates_in_course(p_course_id INT)
LANGUAGE plpgsql
AS $$
DECLARE
    student_rec RECORD;
    cur_students CURSOR FOR
        SELECT 
            s.name, s.email
        FROM 
            students s
        JOIN 
            enrollments e ON s.student_id = e.student_id
        LEFT JOIN 
            certificates c ON e.enrollment_id = c.enrollment_id
        WHERE 
            e.course_id = p_course_id AND c.certificate_id IS NULL;
BEGIN
    OPEN cur_students;

    LOOP
        FETCH cur_students INTO student_rec;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE 'Student: %, Email: %', student_rec.name, student_rec.email;
    END LOOP;

    CLOSE cur_students;
END;
$$;

CALL print_students_without_certificates_in_course(4);



CREATE OR REPLACE PROCEDURE print_all_students_without_certificates()
LANGUAGE plpgsql
AS $$
DECLARE
    student_rec RECORD;
    cur_students CURSOR FOR
        SELECT 
            s.name, s.email, c.course_name
        FROM 
            students s
        JOIN 
            enrollments e ON s.student_id = e.student_id
        JOIN 
            courses c ON e.course_id = c.course_id
        LEFT JOIN 
            certificates cert ON e.enrollment_id = cert.enrollment_id
        WHERE 
            cert.certificate_id IS NULL;
BEGIN
    OPEN cur_students;

    LOOP
        FETCH cur_students INTO student_rec;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE 'Student: %, Email: %, Course: %', student_rec.name, student_rec.email, student_rec.course_name;
    END LOOP;

    CLOSE cur_students;
END;
$$;

CALL print_all_students_without_certificates();

-- Phase 6: Security & Roles

/* 1. Create a `readonly_user` role:

   * Can run `SELECT` on `students`, `courses`, and `certificates`
   * Cannot `INSERT`, `UPDATE`, or `DELETE` 

*/

CREATE ROLE readonly_user LOGIN PASSWORD 'readonly_pass';

GRANT CONNECT ON DATABASE edtech TO readonly_user;

GRANT SELECT ON students, courses, certificates TO readonly_user;

REVOKE INSERT, UPDATE, DELETE ON students, courses, certificates FROM readonly_user;


/*
2. Create a `data_entry_user` role:

   * Can `INSERT` into `students`, `enrollments`
   * Cannot modify certificates directly

*/

CREATE ROLE data_entry_user LOGIN PASSWORD 'dataentry_pass';

GRANT INSERT ON students, enrollments TO data_entry_user;

REVOKE ALL ON certificates FROM data_entry_user;

REVOKE UPDATE, DELETE ON students, enrollments FROM data_entry_user;

REVOKE CREATE ON SCHEMA public FROM readonly_user, data_entry_user;

GRANT USAGE, SELECT ON SEQUENCE students_student_id_seq TO data_entry_user;
GRANT USAGE, SELECT ON SEQUENCE enrollments_enrollment_id_seq TO data_entry_user;

-- Phase 7: Transactions & Atomicity
/*
A transaction block that:

* Enrolls a student
* Issues a certificate
* Fails if certificate generation fails (rollback)
*/

DO $$
DECLARE
  v_enrollment_id INT;
BEGIN
  -- Start transaction 
  -- Insert enrollment
  CALL insert_enrollment(2, 5, CURRENT_DATE);
  
  SELECT enrollment_id INTO v_enrollment_id
  FROM enrollments
  WHERE student_id = 2 AND course_id = 5
  ORDER BY enroll_date DESC
  LIMIT 1;

  -- Insert certificate using the enrollment_id
  CALL insert_certificate(v_enrollment_id, CURRENT_DATE, 'CERT-' || v_enrollment_id || '-' || to_char(CURRENT_DATE, 'YYYYMMDD'));

  -- If all succeed, transaction commits automatically

EXCEPTION
  WHEN OTHERS THEN
    RAISE NOTICE 'Transaction failed: %, rolling back', SQLERRM;
    ROLLBACK;
    RAISE;  -- re-throw error after rollback
END;
$$;

SELECT * from enrollments
SELECT * FROM certificates
