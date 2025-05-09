--1. List all films with their length and rental rate, sorted by length descending.
SELECT title, length, rental_rate
FROM film
ORDER BY length DESC;

--2. Find the top 5 customers who have rented the most films.
SELECT
    c.customer_id,
    c.first_name || ' ' || c.last_name AS full_name,
    COUNT(r.rental_id) AS total_rentals
FROM customer c
JOIN rental r ON c.customer_id = r.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
ORDER BY total_rentals DESC
LIMIT 5;

--3. Display all films that have never been rented.
SELECT f.title
FROM film f
LEFT JOIN inventory i ON f.film_id = i.film_id 
LEFT JOIN rental r ON i.inventory_id = r.inventory_id
WHERE r.rental_id IS NULL;

--4. List all actors who appeared in the film ‘Academy Dinosaur’.
SELECT 	
	  a.first_name || ' ' || a.last_name AS actor_name
	  FROM actor a
	  LEFT JOIN film_actor fa ON fa.actor_id = a.actor_id
	  LEFT JOIN film f ON f.film_id = fa.film_id
	  WHERE f.title = 'Academy Dinosaur'

--5. List each customer along with the total number of rentals they made and the total amount paid.
SELECT 
	c.first_name || ' ' || c.last_name AS customer_name,
	COUNT (r.rental_id) as Total_Number_of_Rentals,
	SUM (p.amount) as Total_Amount
	FROM customer c 
	LEFT JOIN rental r ON c.customer_id = r.customer_id
	LEFT JOIN payment p ON p.rental_id = r.rental_id 
	GROUP BY c.customer_id, c.first_name, c.last_name
	ORDER BY Total_Amount DESC 

--6. Using a CTE, show the top 3 rented movies by number of rentals.

WITH RentalCounts AS (
    SELECT 
        f.title,
        COUNT(r.rental_id) AS rental_count
    FROM film f
    JOIN inventory i ON f.film_id = i.film_id
    JOIN rental r ON i.inventory_id = r.inventory_id
    GROUP BY f.title
)
SELECT 
    title, 
    rental_count
FROM RentalCounts
ORDER BY rental_count DESC
LIMIT 3;

--7. Find customers who have rented more than the average number of films. Use a CTE to compute the average rentals per customer, then filter.

WITH average_rentals AS (
    SELECT 
        AVG(rental_count) AS avg_rentals 
    FROM (
        SELECT 
            c.customer_id,
            COUNT(r.rental_id) AS rental_count
        FROM customer c
        LEFT JOIN rental r ON c.customer_id = r.customer_id
        GROUP BY c.customer_id
    ) AS customer_rentals
)
SELECT 
    c.first_name || ' ' || c.last_name AS customer_name,
    COUNT(r.rental_id) AS total_rentals
FROM customer c
LEFT JOIN rental r ON c.customer_id = r.customer_id
JOIN average_rentals ar ON 1=1
GROUP BY c.customer_id, c.first_name, c.last_name, ar.avg_rentals
HAVING COUNT(r.rental_id) > (SELECT avg_rentals FROM average_rentals)
ORDER BY total_rentals DESC;

--8. Write a function that returns the total number of rentals for a given customer ID.

CREATE OR REPLACE FUNCTION get_total_rentals(customer_id INT)
RETURNS INT AS $$
DECLARE
    total_rentals INT;
BEGIN
    SELECT COUNT(*) 
    INTO total_rentals
    FROM rental r
    WHERE r.customer_id = $1;  	
    RETURN total_rentals;
END;
$$ LANGUAGE plpgsql;


SELECT get_total_rentals(1);

--9. Write a stored procedure that updates the rental rate of a film by film ID and new rate.

CREATE OR REPLACE PROCEDURE update_rental_rate(film_id INT, new_rate NUMERIC)
AS $$
BEGIN
    UPDATE film
    SET rental_rate = new_rate
    WHERE film.film_id = $1; 
END;
$$ LANGUAGE plpgsql;


CALL update_rental_rate(1, 5.99);

SELECT film_id , rental_rate from film where film_id = 1;
