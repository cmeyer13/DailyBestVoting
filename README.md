# Second Street Voting Automation

A C# console application designed to automate voting on the Second Street platform (`embed-1176068.secondstreetapp.com`) using multiple accounts with realistic browser headers.

## Features
*   **Multi-Account Rotation:** Each email gets its own unique session token, context token, and cookies (mimicking a real browser).
*   **Anti-Detection Headers:** Sends `User-Agent`, `Accept-Encoding` (Brotli/Zstd), `X-Referring-Url`, and `Sec-Fetch-*` headers exactly like Chrome does.
*   **Automatic Retry Logic:** Deletes existing votes before casting new ones to ensure accuracy.
*   **Background Timer:** Runs on a 24-hour loop by default.

## Prerequisites
1.  **.NET SDK** (Version 6.0 or higher recommended).
2.  **NuGet Packages:** Ensure the following packages are installed in your project:
    *   `Microsoft.Extensions.Configuration` (and `.Json`)
    *   `Microsoft.Extensions.Configuration.Binder`
    *   *(Optional)* `Newtonsoft.Json` - *Not required if using default `System.Net.Http.Json`.*

## Setup & Configuration

### 1. Create `appsettings.json`
Create a file named `appsettings.json` in your project root (set to **Copy to Output Directory: Copy always**) and add the following structure:

```json
{
	"Accounts": [
		{
			"Email": "email1@example.com",
			"AccessToken": "999f13b7233428e7",
			"ContextToken": "NTHf-FEd9oDTfp7aiZQWoaADUjmv-MlB1-yAFC_OJZ3sICXFauomO11eketDa2ckkz06h3N1lyMQgGzSOGnt9jtHrNcNi5uqTjUTrRpXcLRyqZzJxKbfncvGuDSJrAxJipRI0ulvUth6PTihEfqxUxLXb2-CeNyPDhPHj2dVgSF4uQNVC69LDvZO3TUhFEWxDM_RWG2n-1e62RAbmsCWhSkyNJoGXwP2vPqIp7QLGj0",
			"Cookies": "guest_idv1%3A...your-full-cookie-string-here..."
		},
		{
			"Email": "email2@example.com",
			"AccessToken": "c1aeaa4d553e9127",
			"ContextToken": "eEsG6wZio13MgvLako8_w07GSjGanp4DoU0NnhJBAnPHwrADw4hDqwUOqM01tNML7eAiYohQNCNDdFIUR-nuY88fmtnNxcH18lgLmUXkm99QOSwb7e1QZ_5RNWmRkNQJko1TPLo-3Zrh4JVkru49fQddyeTPcMzyclBmTFWpyegrRVfNnKEjH7pxguT1Sk_C553h
```
