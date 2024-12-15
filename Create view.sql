CREATE VIEW books_info AS
 SELECT b.isbn,
    b.book_name,
    g.genre_name,
    a.surname::text || ' ' || a.name || ' ' || COALESCE(a.patronymic, '') AS author_full_name,
    p.publishing_house_name,
    b.publishing_year
   FROM books b
     JOIN genres g USING (genre_id)
     LEFT JOIN authors a USING (author_id)
     JOIN publishing_houses p USING (publishing_house_id)
  ORDER BY b.isbn;


CREATE VIEW books_in_stock_summary AS
 SELECT l.library_name,
    l.address,
    sum(bs.total_quantity) AS total_books_count,
    sum(bs.now_in_stock) AS now_in_stock
   FROM books_in_stock bs
     JOIN libraries l USING (library_id)
  GROUP BY l.library_name, l.address
 HAVING sum(bs.total_quantity) > 0
  ORDER BY l.library_name, total_books_count DESC, now_in_stock DESC;
  
  
CREATE VIEW genres_in_stock AS
	SELECT 
		l.library_name,
		l.address,
		genres.genre_name,
		sum(bs.total_quantity) AS total_books_count,
		sum(bs.now_in_stock) AS now_in_stock
	FROM 
		books_in_stock bs
		JOIN libraries l USING (library_id)
		JOIN books b ON bs.book_isbn = b.isbn
		JOIN genres USING (genre_id)
	GROUP BY 
		l.library_name, l.address, genres.genre_name
	HAVING 
		sum(bs.total_quantity) > 0
	ORDER BY 
		l.library_name, total_books_count DESC, now_in_stock DESC;

  
CREATE VIEW popular_books AS
	SELECT 
		b.isbn,
		b.book_name,
		(a.surname || ' ' || a.name || ' ' || COALESCE(a.patronymic, '')) AS author_full_name,
		count(s.subscription_id) AS subscription_count
	FROM 
		books b
		JOIN subscriptions s ON b.isbn = s.book_isbn
		LEFT JOIN authors a USING (author_id)
	GROUP BY 
		b.isbn, b.book_name, author_full_name
	ORDER BY 
		subscription_count DESC;


CREATE VIEW active_subscriptions AS
	SELECT 
		s.subscription_id,
		(r.surname || ' ' || r.name || ' ' || COALESCE(r.patronymic, '')) AS reader_full_name,
		r.phone_number,
		b.book_name,
		l.library_name,
		l.address AS library_address,
		s.issue_date,
		s.return_date,
		CURRENT_DATE > s.return_date AS is_overdue
	FROM 
		subscriptions s
		JOIN libraries l USING (library_id)
		JOIN books b ON s.book_isbn = b.isbn
		JOIN readers r USING (reader_id)
	WHERE 
		s.is_active = true
	ORDER BY 
		reader_full_name, s.book_isbn, l.library_name;


CREATE VIEW overdue_subscriptions AS
	SELECT 
		a.subscription_id,
		a.reader_full_name,
		a.phone_number,
		a.book_name,
		a.library_name,
		a.library_address,
		a.issue_date,
		a.return_date
	FROM 
		active_subscriptions a
	WHERE 
		a.is_overdue = true;
