# Resilient Form Automation (Task 1)

## Project Overview

This project implements a resilient web automation solution using C# and Selenium WebDriver.

The objective is to identify and interact with web form fields without depending on hardcoded IDs, names, or CSS selectors. The solution uses a custom scoring algorithm to locate form elements based on their semantic information.

## Technology Stack

- **Language:** C# (.NET 10)
- **Automation Framework:** Selenium WebDriver
- **Dependencies:**
  - Selenium.Support
  - Microsoft.Extensions.Configuration.Json

## Problem Statement and Solution Architecture

Traditional automation scripts often break when developers modify element IDs, CSS classes, or webpage layouts.

To solve this problem, the project implements a custom `ResilientLocator` class that identifies form fields using a weighted scoring algorithm.

When searching for a field such as "First Name", the algorithm evaluates available input elements based on:

1. Associated label text.
2. Placeholder attributes.
3. Name attributes.
4. ARIA-label attributes.
5. Surrounding parent element text.

Each element receives a confidence score based on the available matching information.

The element with the highest confidence score is selected. If multiple elements have the same highest score or the confidence score is too low, the script reports an ambiguity error instead of interacting with an uncertain element.

## Installation and Configuration

1. Clone the repository.
2. Ensure that the .NET 10 SDK is installed.
3. Navigate to the project directory.
4. Configure the target URL in `appsettings.json`:

```json
{
  "TargetUrl": "https://demoqa.com/automation-practice-form"
}
```

5. Restore the project dependencies:

```bash
dotnet restore
```

## How to Run

Execute the following command from the project root directory:

```bash
dotnet run
```

The browser will launch, navigate to the configured URL, and automatically identify and interact with the form fields using the resilient scoring algorithm.

## Key Features

- Dynamic form field identification.
- No dependency on fixed element IDs or CSS selectors.
- Weighted semantic matching algorithm.
- Ambiguity detection.
- Safe error handling.
- Configurable target URL.
