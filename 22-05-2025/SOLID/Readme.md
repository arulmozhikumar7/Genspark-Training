**SOLID Principles Used**

**1. Single Responsibility Principle (SRP)**  
A class should have only one reason to change, meaning it should have only one responsibility.  
**Example:**  
- `TeamManager` handles player and match stat management only.  
- `ConsoleNotifier` handles notification logic only.  
- Each milestone rule class (`HattrickMilestoneRule`, `AssistMilestoneRule`, `PassMilestoneRule`) focuses on evaluating a single milestone condition.

**2. Open/Closed Principle (OCP)**  
Classes, modules, functions should be open for extension but closed for modification.  
**Example:**  
- New milestone rules can be added by implementing `IMilestoneRule` without modifying existing classes like `MilestoneChecker` or `TeamManager`.

**3. Liskov Substitution Principle (LSP)**  
Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program.  
**Example:**  
- Any implementation of `INotifier` (e.g., `ConsoleNotifier`) or `IMilestoneRule` can be substituted without breaking the `MilestoneChecker` functionality.

**4. Interface Segregation Principle (ISP)**  
Clients should not be forced to depend on interfaces they do not use; create smaller, specific interfaces rather than large, general ones.  
**Example:**  
- Separate interfaces for `IPlayerRepository`, `IStatRepository`, `INotifier`, and `IMilestoneRule` ensure each component depends only on the functionality it needs.

**5. Dependency Inversion Principle (DIP)**  
High-level modules should not depend on low-level modules; both should depend on abstractions (interfaces). Abstractions should not depend on details. Details should depend on abstractions.  
**Example:**  
- `TeamManager` depends on abstractions (`IPlayerRepository`, `IStatRepository`, `MilestoneChecker`) rather than concrete implementations.  
- `MilestoneChecker` depends on `IMilestoneRule` abstractions, not concrete milestone rules.  
- `ConsoleNotifier` implements the `INotifier` interface, allowing flexible notification methods.
