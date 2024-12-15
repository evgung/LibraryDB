CREATE TABLE libraries (
	library_id serial PRIMARY KEY,
	library_name varchar(30) NOT NULL,
	address text
);


CREATE TABLE genres (
	genre_id serial PRIMARY KEY,
	genre_name varchar(30) NOT NULL UNIQUE
);


CREATE TABLE authors (
	author_id serial PRIMARY KEY,
	surname varchar(30) NOT NULL,
	"name" varchar(30) NOT NULL,
	patronymic varchar(30)
);


CREATE TABLE publishing_houses (
	publishing_house_id serial PRIMARY KEY,
	publishing_house_name varchar(30) NOT NULL
);


CREATE TABLE books (
	ISBN varchar(17) PRIMARY KEY,
	book_name text NOT NULL,
	genre_id integer NOT NULL,
	author_id integer,
	publishing_house_id integer NOT NULL,
	publishing_year integer NOT NULL,
	
	FOREIGN KEY (genre_id) REFERENCES genres(genre_id),
	FOREIGN KEY (author_id) REFERENCES authors(author_id),
	FOREIGN KEY (publishing_house_id) REFERENCES publishing_houses(publishing_house_id),
	UNIQUE (ISBN, book_name)
);


CREATE TABLE books_in_stock (
	"id" serial PRIMARY KEY,
	library_id integer NOT NULL,
	book_ISBN varchar(17) NOT NULL,
	total_quantity integer NOT NULL DEFAULT 0,
	now_in_stock integer NOT NULL DEFAULT 0,
	
	FOREIGN KEY (library_id) REFERENCES libraries(library_id)
		ON DELETE CASCADE,
	FOREIGN KEY (book_ISBN) REFERENCES books(ISBN)
		ON UPDATE CASCADE,
	UNIQUE (library_id, book_ISBN),
	CHECK (now_in_stock <= total_quantity)
);


CREATE TABLE readers (
	reader_id serial PRIMARY KEY,
	surname varchar(30) NOT NULL,
	"name" varchar(30) NOT NULL,
	patronymic varchar(30),
	address text,
	phone_number varchar(30)
);


CREATE TABLE subscriptions (
	subscription_id serial PRIMARY KEY,
	library_id integer NOT NULL,
	book_ISBN varchar(17) NOT NULL,
	reader_id integer NOT NULL,
	issue_date date NOT NULL,
	return_date date NOT NULL,
	is_active boolean NOT NULL DEFAULT TRUE,
	
	FOREIGN KEY (library_id) REFERENCES libraries(library_id)
		ON DELETE CASCADE,
	FOREIGN KEY (book_ISBN) REFERENCES books(ISBN)
		ON UPDATE CASCADE,
	FOREIGN KEY (reader_id) REFERENCES readers(reader_id)
		ON DELETE CASCADE,
	CHECK (issue_date < return_date)
);


CREATE INDEX ON books(genre_id);
CREATE INDEX ON books(author_id);
CREATE INDEX ON books(publishing_house_id);
CREATE INDEX ON books_in_stock(library_id);
CREATE INDEX ON books_in_stock(book_isbn);
CREATE INDEX ON subscriptions(library_id);
CREATE INDEX ON subscriptions(book_ISBN);
CREATE INDEX ON subscriptions(reader_id);
CREATE INDEX ON subscriptions(library_id, reader_id);
