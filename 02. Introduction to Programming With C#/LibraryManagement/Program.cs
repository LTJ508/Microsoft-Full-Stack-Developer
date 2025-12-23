class LibraryManager
{
    // Book structure to track title and checkout status
    class Book
    {
        public string Title { get; set; }
        public bool IsCheckedOut { get; set; }

        public Book(string title)
        {
            Title = title;
            IsCheckedOut = false;
        }
    }

    static void Main()
    {
        // Using an array to store books for easier management
        Book[] books = new Book[10]; // Increased capacity for more books
        int borrowedCount = 0; // Track number of borrowed books
        const int MAX_BORROWED_BOOKS = 3; // Borrowing limit

        while (true)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("Library Management System");
            Console.WriteLine("========================================");
            Console.WriteLine("Options: add | remove | search | checkout | checkin | list | exit");
            Console.Write("Enter your choice: ");
            string action = GetUserInput()?.ToLower().Trim() ?? "";

            switch (action)
            {
                case "add":
                    AddBook(books);
                    break;
                case "remove":
                    RemoveBook(books);
                    break;
                case "search":
                    SearchBook(books);
                    break;
                case "checkout":
                    borrowedCount = CheckoutBook(books, borrowedCount, MAX_BORROWED_BOOKS);
                    break;
                case "checkin":
                    borrowedCount = CheckinBook(books, borrowedCount);
                    break;
                case "list":
                    // Just display books without prompting for action again
                    break;
                case "exit":
                    Console.WriteLine("\nExiting the library management system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("\nInvalid action. Please choose a valid option.");
                    break;
            }

            // Display books after each operation
            DisplayBooks(books, borrowedCount, MAX_BORROWED_BOOKS);
        }
    }

    /// <summary>
    /// Gets user input from the console with null safety
    /// </summary>
    static string GetUserInput()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    /// <summary>
    /// Adds a book to the library if space is available
    /// </summary>
    static void AddBook(Book[] books)
    {
        // Check if library is full
        int emptySlot = FindEmptySlot(books);
        
        if (emptySlot == -1)
        {
            Console.WriteLine("\nThe library is full. No more books can be added.");
            Console.WriteLine($"Maximum capacity: {books.Length} books");
            return;
        }

        Console.Write($"\nEnter the title of the book to add: ");
        string newBook = GetUserInput()?.Trim() ?? "";

        // Validate input
        if (string.IsNullOrWhiteSpace(newBook))
        {
            Console.WriteLine("\nError: Book title cannot be empty.");
            return;
        }

        // Check for duplicate books (case-insensitive)
        if (BookExists(books, newBook))
        {
            Console.WriteLine($"\nError: '{newBook}' already exists in the library.");
            return;
        }

        books[emptySlot] = new Book(newBook);
        Console.WriteLine($"\nSuccess: '{newBook}' has been added to the library.");
    }

    /// <summary>
    /// Removes a book from the library by title (case-insensitive)
    /// </summary>
    static void RemoveBook(Book[] books)
    {
        // Check if library is empty
        if (IsLibraryEmpty(books))
        {
            Console.WriteLine("\nThe library is empty. No books to remove.");
            return;
        }

        Console.Write("\nEnter the title of the book to remove: ");
        string removeBook = GetUserInput()?.Trim() ?? "";

        // Validate input
        if (string.IsNullOrWhiteSpace(removeBook))
        {
            Console.WriteLine("\nError: Book title cannot be empty.");
            return;
        }

        // Find and remove the book (case-insensitive search)
        int bookIndex = FindBookIndex(books, removeBook);
        
        if (bookIndex != -1)
        {
            string removedTitle = books[bookIndex].Title;
            books[bookIndex] = null;
            Console.WriteLine($"\nSuccess: '{removedTitle}' has been removed from the library.");
        }
        else
        {
            Console.WriteLine($"\nError: Book '{removeBook}' not found in the library.");
        }
    }

    /// <summary>
    /// Searches for a book by title (case-insensitive)
    /// </summary>
    static void SearchBook(Book[] books)
    {
        Console.Write("\nEnter the title of the book to search: ");
        string searchTitle = GetUserInput()?.Trim() ?? "";

        // Validate input
        if (string.IsNullOrWhiteSpace(searchTitle))
        {
            Console.WriteLine("\nError: Search term cannot be empty.");
            return;
        }

        int bookIndex = FindBookIndex(books, searchTitle);

        if (bookIndex != -1)
        {
            Book book = books[bookIndex];
            Console.WriteLine($"\n✓ Found: '{book.Title}'");
            Console.WriteLine($"Status: {(book.IsCheckedOut ? "Checked Out" : "Available")}");
        }
        else
        {
            Console.WriteLine($"\n✗ Not Found: '{searchTitle}' is not in the library collection.");
        }
    }

    /// <summary>
    /// Checks out a book from the library
    /// </summary>
    static int CheckoutBook(Book[] books, int borrowedCount, int maxBorrowed)
    {
        // Check if user has reached borrowing limit
        if (borrowedCount >= maxBorrowed)
        {
            Console.WriteLine($"\nError: You have reached the maximum borrowing limit of {maxBorrowed} books.");
            Console.WriteLine("Please check in a book before borrowing another.");
            return borrowedCount;
        }

        // Check if there are available books
        if (IsLibraryEmpty(books))
        {
            Console.WriteLine("\nThe library is empty. No books to check out.");
            return borrowedCount;
        }

        Console.Write("\nEnter the title of the book to check out: ");
        string checkoutTitle = GetUserInput()?.Trim() ?? "";

        // Validate input
        if (string.IsNullOrWhiteSpace(checkoutTitle))
        {
            Console.WriteLine("\nError: Book title cannot be empty.");
            return borrowedCount;
        }

        int bookIndex = FindBookIndex(books, checkoutTitle);

        if (bookIndex == -1)
        {
            Console.WriteLine($"\nError: Book '{checkoutTitle}' not found in the library.");
            return borrowedCount;
        }

        Book book = books[bookIndex];

        if (book.IsCheckedOut)
        {
            Console.WriteLine($"\nError: '{book.Title}' is already checked out.");
            return borrowedCount;
        }

        book.IsCheckedOut = true;
        borrowedCount++;
        Console.WriteLine($"\nSuccess: '{book.Title}' has been checked out.");
        Console.WriteLine($"You have borrowed {borrowedCount}/{maxBorrowed} books.");

        return borrowedCount;
    }

    /// <summary>
    /// Checks in a borrowed book
    /// </summary>
    static int CheckinBook(Book[] books, int borrowedCount)
    {
        // Check if user has any borrowed books
        if (borrowedCount == 0)
        {
            Console.WriteLine("\nYou don't have any books checked out.");
            return borrowedCount;
        }

        Console.Write("\nEnter the title of the book to check in: ");
        string checkinTitle = GetUserInput()?.Trim() ?? "";

        // Validate input
        if (string.IsNullOrWhiteSpace(checkinTitle))
        {
            Console.WriteLine("\nError: Book title cannot be empty.");
            return borrowedCount;
        }

        int bookIndex = FindBookIndex(books, checkinTitle);

        if (bookIndex == -1)
        {
            Console.WriteLine($"\nError: Book '{checkinTitle}' not found in the library.");
            return borrowedCount;
        }

        Book book = books[bookIndex];

        if (!book.IsCheckedOut)
        {
            Console.WriteLine($"\nError: '{book.Title}' is not checked out. It's already available in the library.");
            return borrowedCount;
        }

        book.IsCheckedOut = false;
        borrowedCount--;
        Console.WriteLine($"\nSuccess: '{book.Title}' has been checked in. Thank you!");
        Console.WriteLine($"You now have {borrowedCount} book(s) checked out.");

        return borrowedCount;
    }

    /// <summary>
    /// Displays all books currently in the library with their status
    /// </summary>
    static void DisplayBooks(Book[] books, int borrowedCount, int maxBorrowed)
    {
        Console.WriteLine("\n--- Library Catalog ---");
        
        int availableCount = 0;
        int checkedOutCount = 0;
        int totalCount = 0;

        for (int i = 0; i < books.Length; i++)
        {
            if (books[i] != null)
            {
                totalCount++;
                string status = books[i].IsCheckedOut ? "[CHECKED OUT]" : "[AVAILABLE]";
                Console.WriteLine($"{totalCount}. {books[i].Title} {status}");

                if (books[i].IsCheckedOut)
                    checkedOutCount++;
                else
                    availableCount++;
            }
        }

        if (totalCount == 0)
        {
            Console.WriteLine("(No books in the library)");
        }
        
        Console.WriteLine($"\n--- Statistics ---");
        Console.WriteLine($"Total books in library: {totalCount}/{books.Length}");
        Console.WriteLine($"Available: {availableCount} | Checked out: {checkedOutCount}");
        Console.WriteLine($"Your borrowed books: {borrowedCount}/{maxBorrowed}");
        Console.WriteLine($"Available borrowing slots: {maxBorrowed - borrowedCount}");
    }

    /// <summary>
    /// Finds the first empty slot in the library
    /// </summary>
    /// <returns>Index of empty slot, or -1 if library is full</returns>
    static int FindEmptySlot(Book[] books)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (books[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Finds the index of a book by title (case-insensitive)
    /// </summary>
    /// <returns>Index of the book, or -1 if not found</returns>
    static int FindBookIndex(Book[] books, string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (books[i] != null && 
                books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Checks if a book already exists in the library (case-insensitive)
    /// </summary>
    static bool BookExists(Book[] books, string title)
    {
        return FindBookIndex(books, title) != -1;
    }

    /// <summary>
    /// Checks if the library has no books
    /// </summary>
    static bool IsLibraryEmpty(Book[] books)
    {
        foreach (Book book in books)
        {
            if (book != null)
            {
                return false;
            }
        }
        return true;
    }
}