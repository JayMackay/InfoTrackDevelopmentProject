# Keyword Web Scraper Application

This is a fullstack ASP.NET Core web application with a React frontend. It is designed to perform web searches by querying selected search engines with a specific search string. The application retrieves and analyzes the search results, identifying how many pages contain the specified search string and matches it to a given URL. It then returns the page numbers of where the search string and URL combination occurs

The robust user interface, built with React, enables seamless interaction with the service, while the ASP.NET Core backend handles search logic and integrates with various search engines. Following the Model View Controller (MVC) pattern, the application ensures a clear separation of concerns, enhancing maintainability and scalability. Dependency injection is employed to promote testability and flexibility within the application's architecture

To submit bug reports, feature suggestions, or track changes, visit the GitHub repository at https://github.com/JayMackay/InfoTrackDevelopmentProject

## Contents Of This File

- Requirements
- Features
- Installation
- Problem
- Adding Additional Search Engines

## Requirements

This project requires the following:

- **Backend:** .NET 7.0. Ensure you have the necessary version installed to build and run the backend, which can be found [HERE](https://dotnet.microsoft.com/download/dotnet/7.0)
- **Frontend:** Node.js (recommended version 18.x). Download from [HERE](https://nodejs.org/en/download/package-manager)

Git link for cloning the repository: `https://github.com/JayMackay/InfoTrackDevelopmentProject.git`

## Features

This application includes:

- **Backend (ASP.NET Core):**
  - **MVC Pattern:** Implements the Model View Controller approach to separate concerns and streamline development
  - **Multiple Search Engines:** Supports querying multiple search engines, including Google, Bing and Yahoo
  - **Enhanced Error Handling:** Improved error handling with detailed logging for various HTTP response codes and connection issues
  - **Dependency Injection:** Utilizes dependency injection to manage services and improve testability
  - **Configuration Management:** Uses `appsettings.json` for dynamic configuration of search engine URLs

- **Frontend (React):**
  - **Interactive UI:** Provides a user friendly interface for submitting search queries and displaying results
  - **Form Handling:** Manages form inputs for keywords, URL, and search engine selection
  - **Loading and Error States:** Displays loading indicators and error messages to enhance user experience

## Installation

### Step 1: Clone the Repository

Clone the repository to your local machine using Git:

```
git clone <repository-url>
```

_Replace <repository-url> with the actual URL of the Git repository_

### Step 2: Navigate to the Project Directory

Open a terminal or command prompt and navigate into the project directory:

### Step 3: Build the Solution**

Build the solution using Visual Studio or .Net CLI:

Using Visual Studio:
Open the solution file (CreditReferencingConsoleApplication.sln) in Visual Studio and build the solution

Using .Net CLI
Run the following command in your terminal or command prompt:

```
dotnet build
```

### Step 4: Frontend Setup

1. **Navigate to the Frontend Project Directory**

   Open a terminal or command prompt and navigate into the frontend project directory. For example:

   ```bash
   cd path\to\frontend\project
   ```
   
2. Install Dependencies

Install the required Node.js packages using npm:

```bash
npm install
```

3. Run the Frontend

Start the React development server:

```bash
npm start
```
#### Notes

**Frontend Localhost Port:**

- By default, the React development server runs on port 3000
- If port 3000 is already in use or unavailable, React will automatically select the next available port
- The chosen port may vary depending on your machine and environment
- If needed, you can customize the port number in the React project's configuration

### Step 5: Configure `appsettings.json`

Open `appsettings.json` located in the main project directory and update the configuration for `SearchEngines` to include the URLs for your desired search engines.

Example `appsettings.json` snippet:

```json
{
"Logging": {
 "LogLevel": {
   "Default": "Information",
   "Microsoft": "Warning",
   "Microsoft.AspNetCore.SpaProxy": "Information",
   "Microsoft.Hosting.Lifetime": "Information"
 }
},
"SearchEngines": {
 "Google": "https://www.google.com/search?q=",
 "Bing": "https://www.bing.com/search?q=",
 "Yahoo": "https://uk.search.yahoo.com/search?p="
}
}
```
## Adding Additional Search Engines

To add support for more search engines, follow these steps:

### 1. Update `appsettings.json`

Add the new search engine's URL to the `SearchEngines` section in `appsettings.json`. For example, to add support for "ExampleSearch":

```json
{
  "SearchEngines": {
    "Google": "https://www.google.com/search?q=",
    "Bing": "https://www.bing.com/search?q=",
    "Yahoo": "https://uk.search.yahoo.com/search?p=",
    "ExampleSearch": "https://www.examplesearch.com/search?q="
  }
}
```

### 2. Update the Frontend (React)

Ensure that the frontend allows users to select the new search engine. Modify `SearchEngineDropdown.js` (or the equivalent component) to include the new engine in the options

### 3. Test the Integration

After updating the configuration, test the integration to ensure that the new search engine works correctly with the application
