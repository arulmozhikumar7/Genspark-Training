 AAA Pattern in Unit Testing
| Aspect      | Description                                                             |
| ----------- | ----------------------------------------------------------------------- |
| Arrange     | Setup all necessary objects, mock data, and dependencies                |
| Act         | Call the method or function being tested                                |
| Assert      | Check whether the outcome matches the expected result (validation step) |
| Example     | `var result = service.GetById(1); Assert.AreEqual(expected, result);`   |
| Purpose     | To ensure code is predictable, testable, and follows a clean structure  |


 Role-Based Authorization with JWT
| Feature              | Description                                                        |
| -------------------- | ------------------------------------------------------------------ |
|   Purpose            | Restrict access to APIs or resources based on user roles           |
|   Roles Defined As   | Claims inside the JWT token (`"role": "Admin"`)                    |
|   Setup Required     | Configure JWT Authentication and Role claims                       |
|   Controller Usage   | `[Authorize(Roles = "Admin")]`                                     |
|   Example Claim      | `new Claim(ClaimTypes.Role, "Admin")`                              |
|   Common Use Cases   | Admin panel, feature access based on roles (Admin, User, Editor)   |

Policy-Based Authorization
| Feature                | Description                                                              |
| ---------------------- | ------------------------------------------------------------------------ |
|   Purpose              | Apply complex rules using claims, custom logic, or multiple conditions   |
|   Defined In           | `builder.Services.AddAuthorization(options => options.AddPolicy(...))`   |
|   Applies To           | `[Authorize(Policy = "PolicyName")]`                                     |
|   Custom Requirement   | Create custom `IAuthorizationRequirement` & handler logic                |
|   Example Policy       | `"MinAge18"` checks if the user is at least 18 using `DateOfBirth` claim |

