# OAuth Flow
OAuth is an open standard for access delegation. It allows apps to access user data from another service (like Google) without needing their password.

Typical OAuth Flow:
1. User clicks "Login with Google".
2. Google shows a consent screen.
3. User approves.
4. Google returns an ID token or access token to the frontend.
5. Frontend sends token to backend.
6. Backend verifies token (e.g., using Google APIs).
7. If valid, backend creates/logs in the user.