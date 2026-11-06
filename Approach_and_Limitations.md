# Technical Approach and Limitations

This document explains the technical approach, implementation decisions, and limitations of both automation tasks.

# Task 1: Resilient Web Automation

## Technical Approach

The solution uses a custom weighted scoring algorithm instead of relying on fixed element locators.

The `ResilientLocator` class scans available form input elements and evaluates them using semantic information such as:

- Associated label text.
- Placeholder attributes.
- Name attributes.
- ARIA-label attributes.
- Surrounding parent element text.

A confidence score is calculated for each candidate element. The element with the highest score is selected for interaction.

Regex word boundaries are used to improve text matching and reduce unintended matches.

## Resilience to Webpage Changes

The solution focuses on the meaning and semantic context of form elements rather than their IDs, CSS classes, or DOM positions.

This makes the automation more resilient to changes such as:

- Element ID changes.
- CSS class renaming.
- Changes in element positions.
- Certain HTML structure modifications.

## Ambiguity Handling

The algorithm avoids selecting elements when the result is uncertain.

If multiple elements receive the same highest score, or the confidence score falls below the required threshold, the application reports an ambiguity error.

This prevents the script from interacting with an incorrectly identified field.

## Limitations

1. **Performance:** Scanning and scoring multiple DOM elements requires more processing than locating an element directly by ID.
2. **Custom Controls:** Some JavaScript-based controls may not expose standard HTML input elements. These controls may require specialized handling.
3. **Semantic Dependencies:** The accuracy of the algorithm depends on the available labels, placeholders, and surrounding text.

# Task 2: AI-Powered Test Case Automation

## Technical Approach

The solution separates natural language understanding from browser execution.

Google Gemini converts plain English test cases into structured JSON actions using a predefined prompt and expected action schema.

The Playwright runner validates the generated actions and executes them using generic browser locators.

Supported actions include:

- Navigation.
- Filling input fields.
- Clicking elements.
- Selecting options.
- Verifying text.
- Checking element visibility.

## Validation and Execution

Before browser execution, the generated JSON is validated against the expected action structure.

Playwright then executes each action and records its result.

If an element cannot be located within the configured timeout, the application handles the error and reports the test as failed instead of terminating unexpectedly.

## Limitations

1. **AI Hallucinations:** Gemini may occasionally generate incorrect actions or identify the wrong element when instructions are unclear.
2. **Complex Verification:** The current implementation focuses on basic text and element visibility checks. More advanced application-state verification could be added.
3. **API Dependency:** Generating test actions requires access to the Google Gemini API and a valid API key.
4. **Execution Speed:** Calling the Gemini API introduces additional time before browser execution begins.
5. **Website Variations:** Highly customized interfaces may require additional locator strategies or specialized execution logic.

# Future Improvements

Potential improvements for both projects include:

- Adding more locator strategies.
- Improving confidence scoring.
- Supporting additional custom form controls.
- Introducing more advanced test assertions.
- Adding structured HTML test reports.
- Supporting additional browser engines.
- Improving AI-generated action validation.

# Conclusion

Both projects demonstrate different approaches to making browser automation more reliable.

Task 1 uses semantic element identification and a weighted scoring algorithm to reduce dependency on fixed locators.

Task 2 combines AI-powered natural language processing with Playwright to automate test cases across different websites.

Both solutions prioritize error handling and aim to avoid executing uncertain actions.
