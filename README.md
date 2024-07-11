
# ContactBook Application

This repository contains a WPF ContactBook application developed using .NET Core, DevExpress, and the MVVM pattern. The application provides CRUD operations for managing contacts.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete contacts efficiently.
- **Async Functions**: Enhanced performance with asynchronous operations.
- **Dependency Injection (DI)**: Implemented for better code maintainability and testability.

## Technologies Used

- **.NET Core**: For building a robust and scalable WPF application.
- **WPF (Windows Presentation Foundation)**: For creating a rich and interactive user interface.
- **DevExpress**: For advanced UI controls and components.
- **MVVM (Model-View-ViewModel)**: For separating the development of the graphical user interface.
- **SQLite**: For local database management.
- **Dependency Injection**: To manage dependencies and enhance testability.

## Getting Started

### Prerequisites

- Visual Studio 2019 or later
- .NET Core SDK
- DevExpress Components

### Installation

1. Clone the repository:
    \`\`\`sh
    git clone https://github.com/mustafagoktugibolar/DevContactBook.git
    \`\`\`
2. Navigate to the project directory:
    \`\`\`sh
    cd contactbook
    \`\`\`
3. Open the solution file (\`ContactBook.sln\`) in Visual Studio.

### Database Setup

The application uses SQLite for data storage. Ensure you have SQLite installed and properly configured. You can visually inspect and manage the SQLite database using TablePlus.

### Build and Run

1. Restore the NuGet packages:
    \`\`\`sh
    dotnet restore
    \`\`\`
2. Build the solution:
    \`\`\`sh
    dotnet build
    \`\`\`
3. Run the application:
    \`\`\`sh
    dotnet run
    \`\`\`

## Usage

- **Main Interface**: Manage your contacts by adding, viewing, updating, or deleting entries.

## Contribution

Feel free to fork this repository and contribute by submitting a pull request. For major changes, please open an issue to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgements

- Thanks to the .NET and WPF community for their valuable resources and support.
- Special thanks to DevExpress for providing advanced UI components.
