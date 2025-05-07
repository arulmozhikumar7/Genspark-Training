use pubs

go

select title, pub_name 
from titles join publishers
on titles.pub_id = publishers.pub_id

-- Print the publisher detailers who has never published

select * from publishers where pub_id not in
(select distinct pub_id from titles)

-- Right outer join example

select title, pub_name 
from titles right outer join publishers
on titles.pub_id = publishers.pub_id

-- Select the author_id for all the books . Print the author_id and the book name

SELECT t.title, ta.au_id
FROM titles t
JOIN titleauthor ta ON t.title_id = ta.title_id;

-- Select Author name and the book name

select CONCAT(au_fname,' ',au_lname) as Author_Name , title as Book_Name from authors a
join titleauthor ta on a.au_id = ta.au_id
join titles t on ta.title_id = t.title_id

-- Print Publishers name , Book name and order date of the books

SELECT p.pub_name AS Publisher_Name, 
       t.title AS Book_Name, 
       CONVERT(varchar, s.ord_date, 3) AS Order_Date --3(European format) Represents Style codeformat
FROM publishers p
JOIN titles t ON p.pub_id = t.pub_id
JOIN sales s ON t.title_id = s.title_id
order by 3 desc 

-- Print the publisher name and the first book sale date for all the publishers|

SELECT p.pub_name AS Publisher_Name,
      CONVERT(varchar,  MIN(s.ord_date), 3) AS First_Book_Sale_Date
FROM publishers p
LEFT OUTER JOIN titles t ON p.pub_id = t.pub_id
LEFT OUTER JOIN sales s ON t.title_id = s.title_id
GROUP BY p.pub_name
ORDER BY MIN(s.ord_date) DESC;

-- Print Bookname and the store address of the sale

SELECT t.title AS Book_Name, st.stor_address AS Store_Address
FROM titles t 
JOIN sales s ON t.title_id = s.title_id
JOIN stores st ON st.stor_id = s.stor_id
ORDER BY t.title ASC;


