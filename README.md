
# BookStore API

This API is designed to manage books, authors, and users for an online bookstore. It allows authorized users to manage authors and books, while also providing authentication and user management features like login and registration.

## Features

- **Book Management**: Create, read, update, and delete books.
- **Author Management**: Create, update, view, and delete authors.
- **User Management**: User authentication and registration, role-based access control.
- **Admin Features**: Admin users can create and manage other admins.
- **Security**: Uses JWT for secure authentication and role-based authorization for different actions.

---

## Endpoints

### 1. **Book Management**

#### `GET /api/books`
Fetches all books.
- **Responses**:
  - `200 OK`: Successfully retrieved books.
  - `404 Not Found`: No books found.

#### `GET /api/books/{id}`
Fetches a specific book by its ID.
- **Responses**:
  - `200 OK`: Successfully retrieved the book.
  - `404 Not Found`: Book not found.

#### `POST /api/books`
Creates a new book.
- **Request Body**:
  ```json
  {
    "Title": "Book Title",
    "Description": "Book Description",
    "AuthorId": 1,
    "Price": 19.99
  }
  ```
- **Responses**:
  - `200 OK`: Book created successfully.
  - `400 Bad Request`: Invalid book data.

#### `PUT /api/books`
Updates an existing book.
- **Request Body**:
  ```json
  {
    "Id": 1,
    "Title": "Updated Book Title",
    "Description": "Updated Description",
    "Price": 29.99
  }
  ```
- **Responses**:
  - `200 OK`: Book updated successfully.
  - `404 Not Found`: Book not found.
  - `400 Bad Request`: Invalid book data.

#### `DELETE /api/books/{id}`
Deletes a book by its ID.
- **Responses**:
  - `200 OK`: Book deleted successfully.
  - `404 Not Found`: Book not found.

---

### 2. **Author Management**

#### `GET /api/authors`
Fetches all authors.
- **Responses**:
  - `200 OK`: Successfully retrieved authors.
  - `404 Not Found`: No authors found.

#### `GET /api/authors/{id}`
Fetches a specific author by their ID.
- **Responses**:
  - `200 OK`: Successfully retrieved the author.
  - `404 Not Found`: Author not found.

#### `POST /api/authors`
Creates a new author.
- **Request Body**:
  ```json
  {
    "AuthorFullName": "Author Name",
    "AuthorBIO": "Author Biography",
    "Author_Age": 45,
    "Authors_NumberOfBooks": 10
  }
  ```
- **Responses**:
  - `200 OK`: Author created successfully.
  - `400 Bad Request`: Invalid author data.

#### `PUT /api/authors`
Updates an existing author.
- **Request Body**:
  ```json
  {
    "Id": 1,
    "AuthorFullName": "Updated Author Name",
    "AuthorBIO": "Updated Biography",
    "Author_Age": 50,
    "Authors_NumberOfBooks": 12
  }
  ```
- **Responses**:
  - `200 OK`: Author updated successfully.
  - `404 Not Found`: Author not found.
  - `400 Bad Request`: Invalid author data.

#### `DELETE /api/authors/{id}`
Deletes an author by their ID.
- **Responses**:
  - `200 OK`: Author deleted successfully.
  - `404 Not Found`: Author not found.

---

### 3. **User Management**

#### `POST /api/account/login`
Logs in a user and generates a JWT token.
- **Request Body**:
  ```json
  {
    "Username": "username",
    "Password": "password"
  }
  ```
- **Responses**:
  - `200 OK`: Successfully logged in. Returns JWT token.
  - `400 Bad Request`: Invalid login data.
  - `401 Unauthorized`: Invalid username or password.

#### `POST /api/account/register`
Registers a new user.
- **Request Body**:
  ```json
  {
    "Username": "newuser",
    "Password": "password",
    "Email": "user@example.com"
  }
  ```
- **Responses**:
  - `200 OK`: User registered successfully.
  - `400 Bad Request`: Invalid registration data.

#### `POST /api/account/changepassword`
Allows a logged-in user to change their password.
- **Request Body**:
  ```json
  {
    "OldPassword": "oldpassword",
    "NewPassword": "newpassword"
  }
  ```
- **Responses**:
  - `200 OK`: Password changed successfully.
  - `400 Bad Request`: Invalid password data.
  - `401 Unauthorized`: User not authenticated.

#### `GET /api/account/logout`
Logs out the currently authenticated user.
- **Responses**:
  - `200 OK`: User logged out successfully.
  - `401 Unauthorized`: User not authenticated.

---

### 4. **Admin Management**

#### `POST /api/admin`
Creates a new admin (only accessible by admins).
- **Request Body**:
  ```json
  {
    "Email": "admin@example.com",
    "Username": "newadmin",
    "Password": "password",
    "PhoneNumber": "123-456-7890"
  }
  ```
- **Responses**:
  - `200 OK`: Admin created successfully.
  - `400 Bad Request`: Invalid admin data.

#### `PUT /api/admin`
Edits an admin's profile (only accessible by admins).
- **Request Body**:
  ```json
  {
    "Id": "admin-id",
    "Username": "updatedadmin",
    "Email": "updated@example.com",
    "PhoneNumber": "987-654-3210"
  }
  ```
- **Responses**:
  - `200 OK`: Admin profile updated.
  - `400 Bad Request`: Invalid profile data.

#### `DELETE /api/admin/{id}`
Deletes an admin by ID (only accessible by admins).
- **Responses**:
  - `200 OK`: Admin deleted successfully.
  - `404 Not Found`: Admin not found.

---

## Technologies

- **ASP.NET Core** for building the API.
- **Entity Framework Core** for database operations.
- **AutoMapper** for mapping between DTOs and models.
- **Swashbuckle/Swagger** for API documentation.
- **JWT Authentication** for secure login and authorization.
- **CORS** for cross-origin requests.

---

## Setup

1. Clone the repository:
   ```
   git clone https://github.com/your-repository/bookstore-api.git
   ```

2. Run the project using Visual Studio or any .NET-compatible IDE.

3. Ensure your database is set up and connected. Configure the connection string in `appsettings.json`.

4. Run the application.

---

## Example

### 1. Get all books:
**GET /api/books**

**Response**:
```json
[
    {
        "Id": 1,
        "Title": "Book Title",
        "Description": "Book Description",
        "Price": 19.99,
        "AuthorId": 1
    }
]
```

### 2. Create a new book:
**POST /api/books**

**Request Body**:
```json
{
    "Title": "New Book",
    "Description": "Book Description",
    "AuthorId": 1,
    "Price": 29.99
}
```

**Response**:
```json
{
    "message": "Book created successfully."
}
```

---

## Contribution

Feel free to fork the repository, make changes, and submit pull requests. All contributions are welcome!

