## Getting Started with Unit Testing using NUnit and Moq

### So What is Unit Testing?
Unit testing is a development practice centered around testing software components in isolation from their surroundings and dependencies.  The key focus of Unit Testing is improving software quality by identifying and resolving defects before they are leaked into production.

> [Multiple](http://blog.celerity.com/the-true-cost-of-a-software-bug) studies [have determined](http://xbsoftware.com/blog/cost-bugs-software-testing/) that the cost of fixing a software defect increase drastically the later the defect is discovered.  One of Unit Testing's great advantages lies in identifying defects as early as possible in the development cycle - specifically as code is being written.

#### Are There Other Benefits to Unit Testing?
* Serves as documentation for production code
* Helps to reduce likelihood of introducing breaking changes
* Encourages architectural best practices
* Reduces risk related to changes
* Can be integrated into Continuous Integration environments

#### Are There Drawbacks?
* Learning Curve
* Possible refactoring on brownfield projects
* Additional time spent implementing and maintaining test code
* May be difficult to explain value to Management

>**Note:**  Unit testing is not the only technology available for automated software testing, it fits into a continuum that includes (among many others):
>* Unit Tests:  Test individual components in isolation from environment and concrete dependencies
>* Integration Tests:  Test how concrete components work together within a given subsystem
>* End-to-End Tests:  Test the behavior of an entire system with all parts functioning together
>   * I usually distinguish these tests from Automated UI Tests (see below) if I am driving the system via an API instead of a GUI.
>* Automated UI Tests:  These are commonly used for web-based projects, filling a couple of different roles:
>   * Content-based Testing:  Verifies that the visual, content, and layout properties of a given view are correct
>   * Functional Testing:  Verifies end-to-end functionality by driving the UI and verifying (from the UI layer) that the system functions correctly. 
>
> In a nutshell, one's selection of testing strategies and tools depends on a variety of factors, and tends to vary for each project.  

### So How About a High-Level View?
Sure thing.  Unit testing **does** have several moving parts, but this walkthrough should clear things up:

Abstractly speaking, there are usually three main players involved in unit testing:
* System Under Test (SUT):  This is the code you want to test.  In .NET unit testing, this is usually in a class library.
* Test Suite:  This is a class containing tests that exercise the features of the SUT.
* Test Runner:  This is a tool specific to whichever testing framework you are using.  It provides an environment in which Test Suites can be loaded, executed, and verified.

> Test Suites are usually in their own assembly and are not distributed when pushing an application to production.

**So let's look at one way such an application might be organized:**

* (Visual Studio Solution) MyAwesomeApp
   * (WPF App) MyAwesomeApp.Ui
   * (Class Library) MyAwesomeApp.BusinessLogic
   * (Class Library) MyAwesomeApp.BusinessLogic.Tests
   * (Class Library) MyAwesomeApp.SomeOtherAssembly
   * (Class Library) MyAwesomeApp.SomeOtherAssembly.Tests

For this example, let's assume we're following WPF best practices and avoiding CodeBehind in our UI layer (the main reason being that it's very difficult to test).  Because of this choice, we do not have a test assembly for MyAwesomeApp.Ui.

On the other hand, both MyAwesomeApp.BusinessLogic and MyAwesomeApp.SomeOtherAssembly have associated unit test assemblies, each of which only contains related unit tests - **No application logic goes in unit test assemblies**.

> Note:  There are many schools of thought on what parts of the codebase should be covered by unit tests.  Some environments are very strict, aiming for 100% coverage at all times, while others make pragmatic exceptions such as these:
> * Don't write tests for Plain Old CLR Objects (POCOs) - POCOs are data transfer objects and should not have behaviors.
> * Don't test code that is purely a wrapper over a .NET Framework object (i.e. Directory, File, etc.) - If a class is a pure wrapper (not adding any new or behavior), unit tests end up just testing the .NET Framework, which is not our responsibility.
> 
> In the end, it's up to your team to determine which strategy makes the most sense for your needs.


### OK, So Let's Get Down To It!

> **Prerequisites:**  I'll keep the tools to a minimum, but you'll definitely need a development environment of some sort.  If you're just starting out, I recommend [Visual Studio Community Edition](https://www.visualstudio.com/vs/community/), a free development environment for applications targeting the .NET framework.
> 
>**Important Note for VS Community Users:**  I will be writing tests in this tutorial using the NUnit Library.  Although NUnit has a standalone test runner, it is much more convenient to run the tests within your development environment.  There is a VS Community [Plugin](https://marketplace.visualstudio.com/items?itemName=NUnitDevelopers.NUnit3TestAdapter) that adds support for NUnit 3.x tests to the development environment.

#### Helllllooooooooo NUnit!


### Bonus Material:  So I want to start Unit Testing my existing project...
