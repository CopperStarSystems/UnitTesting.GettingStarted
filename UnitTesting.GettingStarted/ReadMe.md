## Getting Started with Unit Testing using NUnit and Moq


>### Stuck?  Want to Dig Deeper?  
>### Contact me on CodeMentor!
>[![Get help on Codementor](https://cdn.codementor.io/badges/get_help_github.svg)](https://www.codementor.io/copperstarconsulting?utm_source=github&utm_medium=button&utm_term=copperstarconsulting&utm_campaign=github)
 
## Part 1:  A First Look At Unit Testing

### So What is Unit Testing?
Unit testing is a development practice centered around testing software components in isolation from their surroundings and dependencies.  The key focus of Unit Testing is improving software quality by identifying and resolving defects before they are leaked into production.

> [Multiple](http://blog.celerity.com/the-true-cost-of-a-software-bug) studies [have determined](http://xbsoftware.com/blog/cost-bugs-software-testing/) that the cost of fixing a software defect increase drastically the later the defect is discovered.  One of Unit Testing's great advantages lies in identifying defects as early as possible in the development cycle - specifically as code is being written.

#### Are There Other Benefits to Unit Testing?
When applied consistently, unit testing provides a wealth of side benefits:
* Unit tests serve as a form of living documentation for the code, showing each component's intent and clearly communicating intended usage.
  > This contrasts with things like XML documentation which often fall out of date as the application matures.  Tests are constantly written and updated as a normal part of the development cycle, keeping them in closer sync with the application's current state.
* Unit tests reduce the likelihood of introducing breaking changes when implementing new features and bugfixes.  When a breaking change is introduced, it should be exposed when the unit tests are executed.
* Diligent unit testing also encourages architectural best practices, including Dependency Injection, Inversion of Control, and Composition.  Applications written without following these practices are generally very difficult to unit test, usually resulting in a pain point tht is resolved by refactoring to these patterns.
* Unit tests can also be used to determine release readiness, providing great value in agile environments where requirements and ship dates may change unexpectedly.

#### So What Are The Drawbacks?
Although Unit Testing is a powerful tool, it's not a panacea - there can be some downsides to adoption:
* Unit testing involves another set of tools and paradigms to learn, presenting a seemingly-daunting learning curve.  In practice, the fundamental concepts are relatively simple and with some targeted guidance, engineers can quickly come up to speed.
* Unit testing does introduce a certain amount of additional code and maintenance burden - it is, after all, another codebase to maintain.  Usually the most effort is concentrated up front, as the application-specific test suite matures (i.e. implementing base test classes, common infrastructure, etc.), eventually tapering off to a fraction of production development effort.
* If you're adding unit tests to an existing project, there may be significant refactoring involved up front in order to make your classes testable.


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

Abstractly speaking, there are usually four main players involved in unit testing:
* System Under Test (SUT):  This is the code you want to test.  In .NET unit testing, this is usually in a class library.
* Test Suite:  A logical grouping of all the tests for a given system (e.g. "The app's test suite comprises over 9,000 tests")
* Test Fixture:  This is a class containing tests that exercise the features of the SUT.
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

#### Hello NUnit!
So let's write our first, super-simple unit test!

* Start by launching Visual Studio and creating a new Class Library project called UnitTesting.GettingStarted.Tests.  (We're starting with the test library so you can get a taste of NUnit before we dive deeper)
* Delete the Class1.cs file created by Visual Studio
* Go to `Tools -> NuGet Package Manager -> Package Manager Console`, the console will open at the bottom of the development window.
* In the Package Manager Console window, type `Install-Package NUnit` and hit return.  NuGet will install the NUnit libraries we need and reference them in the test project.
* In the Package Manager Console window, type `Install-Package Moq` and hit return.
  >  We'll be using Moq later in the tutorial, so we might as well install it now.
* In Solution Explorer, right-click the project and select `Add -> Class...` and call it HelloNunit.cs
* Update HelloNUnit.cs like so:
  ```c#
    //  --------------------------------------------------------------------------------------
    // UnitTesting.GettingStarted.Tests.HelloNunit.cs
    // 2017/01/12
    //  --------------------------------------------------------------------------------------

    using NUnit.Framework;

    namespace UnitTesting.GettingStarted.Tests
    {
        [TestFixture]
        public class HelloNunit
        {
            [Test]
            public void TestHelloNunit()
            {
                Assert.That(true, Is.False);
            }
        }
    }
  ```

  **So what exactly did we do there?**
  Short Answer:  We wrote our first unit test (which will fail if you run it right now).  Let's break down the structure a bit:

  Notice that the test class (HelloNunit) has a TestFixture attribute attached to it.  This marks the HelloNunit class as containing tests when it is loaded by the NUnit Test Runner.

  We have a similar pattern on the TestHelloNunit() method, but we use the Test attribute instead, to indicate that this method is a test.

  > It's important to know that the Test Class (HelloNunit) and Test Method(TestHelloNunit) names can be whatever you want - the important part is that they be marked with the TestFixture/Test attribute as appropriate.
  > With that in mind though, It's usually best for your Test Method names to clearly describe what they're testing.  I'll have some additional guidance on that coming up shortly.

Within our TestHelloNunit method, we have an Assertion, which is how we verify whether or not a given test should pass.  The general pattern goes like this:  `Assert.That([the actual value], Is.EqualTo([your expected value]))`.  If the assertion evaluates to false, the test fails.

>**A Note About NUnit Assertions:** Although we're using the simple ```Is.EqualTo()``` assertion here, NUnit provides a [wide variety of assertion types](https://github.com/nunit/docs/wiki/Constraint-Model) to suit nearly any assertion need - You can even write custom assertions if your situation requires it. For a complete list of assertion types, see: [the NUnit documentation](https://github.com/nunit/docs/wiki/Constraints).

Sharp eyes may have noticed this statement:  `Assert.That(true, Is.EqualTo(false))` - That's correct, this test cannot pass as written because we're asserting that `true` is equal to `false`.  Go ahead and execute the test (`CTRL+R, A` in Visual Studio) and look what we get in the test results window:

![Failing NUnit Test in Visual Studio IDE](https://cdn.filestackcontent.com/iUo4K4qtSleGMuuuF7Lk "Failing NUnit Test in Visual Studio IDE")

So... Let's go back to HelloNunit.cs and make that test pass.  Change the `Is.False` to `Is.True` and re-run the test - NUnit tells us that all tests passed, so everything is working correctly.

**So wasn't that a little bit pointless?  It looks like we aren't testing anything useful.**

Very true - this exercise was simply to introduce the most basic concepts related to NUnit:  TestFixtures, Tests, and Assertions.  It also illustrated the fact that TestFixtures are classes (decorated with the TestFixtureAttribute), while Tests are methods (decorated with the TestAttribute) on those classes.  Before we dive on in, let's take a quick look at what happens when we run our unit tests:

* The NUnit Test Runner launches and initializes its runtime environment
* The Test Runner analyzes the assembly, looking for classes decorated with the TestFixture attribute
* For each of those classes, the Test Runner looks for methods decorated with the Test attribute and executes them
* When the entire test suite has completed, the Test Runner reports pass/fail statistics for the test run

>**Q:**  Test Suites are usually made up of many tests, so what happens if one fails somewhere in the middle?
>
>**A:**  When a test fails, NUnit notes which test it was (along with other details) and continues running other tests.  When the test run completes, the test results indicate which tests passed vs. failed:
![Unit Test Results](https://cdn.filestackcontent.com/vVlGlimpSouk2POEYNNR "Unit Test Results")

### OK, Now let's do something a little more useful...
So our first example showed you some of NUnit's parts, but it wasn't really a typical use case, so let's flesh it out a bit for a more realistic example.

* Open the UnitTesting.GettingStarted.Tests solution
* Delete the HelloNunit.cs file
* In Solution Explorer, right-click on the solution and select `Add -> New Project...`
* In the New Project dialog, select "Class Library" and call it UnitTesting.GettingStarted
* In the UnitTesting.GettingStarted project, delete the Class1.cs file created by Visual Studio
* In the UnitTesting.GettingStarted.Tests project, add a reference to the UnitTesting.GettingStarted project

**So what did we just do there?**
All that was just setting up the environment.  We cleaned up the HelloNunit.cs file because we don't need it anymore, then we created a project to contain the code we want to test, and then referenced that project from our unit test assembly.

**Now, let's make things sort of interesting...**
> Note:  I'm not following strict Test-Driven Development (TDD) practices here.  If you would like additional information on TDD, please take a look at [The TDD Guy](http://theTddGuy.com).

Let's implement a simple calculator service that we can test:
* In the UnitTesting.GettingStarted project, add a class called Calculator
* Update the class like so:
  ```c#
    //  --------------------------------------------------------------------------------------
    // UnitTesting.GettingStarted.Calculator.cs
    // 2017/01/12
    //  --------------------------------------------------------------------------------------

    namespace UnitTesting.GettingStarted
    {
        public class Calculator
        {
            public int Add(int lhs, int rhs)
            {
                return lhs + rhs;
            }
        }
    }
  ```

* In the UnitTesting.GettingStarted.Tests project, add a class called CalculatorTests and update it like so:
  ```c#
    //  --------------------------------------------------------------------------------------
    // UnitTesting.GettingStarted.Tests.CalculatorTests.cs
    // 2017/01/12
    //  --------------------------------------------------------------------------------------

    using NUnit.Framework;

    namespace UnitTesting.GettingStarted.Tests
    {
        [TestFixture]
        public class CalculatorTests
        {
            [Test]
            public void Add_Always_ReturnsExpectedResult()
            {
                var systemUnderTest = new Calculator();
                Assert.That(systemUnderTest.Add(1, 2), Is.EqualTo(3));
            }
        }
    }
  ```

  > **A Quick Note on Test Names:**  Descriptive test names are very important, particularly as your test suite grows.  I tend to follow this pattern:  `[Subject]_[Scenario]_[Result]`, where:
  > * Subject is usually the name of the method I'm testing - "Add" in this case.
  > * Scenario describes the circumstances that this test covers.  A more complex example might be `GrantLoan_WhenCreditLessThan500_ReturnsFalse`, but in this simple case "Always" will suffice - we don't really ever want unexpected results from our calculator :).
  > * Result describes the expected outcome of invoking the method under test, in this case, the method should return the correct answer
  >
  > As your test suite evolves, descriptive test names can turn into an additional form of developer documentation, outlining the behaviors and expectations of the system in code.
 
 ### Getting a Little Fancier:  Parameterized Tests
 So what if you need to check a particular method multiple times, for different input?  Fortunately, NUnit provides two different ways of parameterizing tests.  Let's say we want to make sure our calculator gives the correct answer for [2,3], [3,5], and [1000,1]:

 Open CalculatorTests.cs and edit the code like so:

```c#
//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.Tests.CalculatorTests.cs
// 2017/01/12
//  --------------------------------------------------------------------------------------

using NUnit.Framework;

namespace UnitTesting.GettingStarted.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 5)]
        [TestCase(1000, 1)]
        public void Add_Always_ReturnsExpectedResult(int lhs, int rhs)
        {
            var systemUnderTest = new Calculator();
            var expected = lhs + rhs;
            Assert.That(systemUnderTest.Add(lhs, rhs), Is.EqualTo(expected));
        }
    }
}
```
Notice that we've changed the `[Test]` attribute to `[TestCase(...)]`, and we've added two arguments to our test method signature.
The parameters to the `TestCase` attribute match up to the test method arguments, and the values are injected when the test is executed.

If you execute the test suite now, you will notice it indicates 4 passing tests - This is because the `TestCase` attribute causes the test to be executed once for each set of values.

> **Note:** A similar pattern you may observe is used for testing edge cases and other scenarios.  For example, imagine if our CreditDecision class from above has logic like so:
> ```c#
>    // Remainder of class omitted for brevity
>    public string MakeCreditDecision(int creditScore){
>        if(creditScore < 550)
>            return "Declined";
>        else if(creditScore <= 675)
>            return "Maybe";
>        else
>            return "We look forward to doing business with you!";
>    }
> ```
> 
> We might need to test that our cutoff points between Declined, Maybe, and Yes answers are being honored by the CreditDecision component. In that case, we could set up tests like so:
> ```c#
> [TestCase(100, "Declined")]
> [TestCase(549, "Declined")]
> [TestCase(550, "Maybe")]
> [TestCase(674, "Maybe")]
> [TestCase(675, "We look forward to doing business with you!")]
> public void MakeCreditDecision_Always_ReturnsExpectedResult(int creditScore, string expectedResult){
>    var result = systemUnderTest.MakeCreditDecision(creditScore);
>    Assert.That(result, Is.EqualTo(expectedResult);
> }
> ```

So we've just barely scratched the surface of NUnit's power, but let's take a quick glance at a related tool: Moq!

### Testing in Isolation:  Moq to the Rescue!
So the examples we've seen so far have been utterly trivial, only testing one class that doesn't have any external dependencies at all.  Although easy to grasp, it doesn't really reflect real-world scenarios.

In a real-world project of any scale, there will be multiple classes working together, sometimes with dependencies on external systems as well.  Although testing against a live system is a valid integration / end-to-end testing strategy, it is not appropriate for unit tests, for example:

>**So What's the Big Deal About Testing in Isolation?**
>
>Imagine if our calculator class didn't do the calculations itself, but relied upon a back-end web service to provide the answers, and that each request to that server takes approximately 1 second to complete.  If we test against a live system, 100 test cases would take 100 seconds to execute due to the back-end latency.  Moreover, just to test one simple aspect of the system would require a fully-functioning backend, no matter what environment is being used for the tests.  Worse yet, because such a test would be based on a chain of possible tiers, a test might fail 'for the wrong reason', i.e. the logic being tested is correct, but some other part of the system (i.e. network communication, etc.) failed.
>.
>
> In the [Github Project]() **TODO:  POST TO GITHUB AND UPDATE LINK** for this tutorial, there is a class called BadCreditDecisionTests that provides additional information on the benefits of testing in isolation.

**So How do we Solve This?**

Ah, such a deceptively simple question :)  In reality, the solution has several parts that work together:
* Dependency Injection - Allows us to specify alternate implementations of dependencies when testing
* 'Programming to the Abstraction' - Whenever possible, specify dependencies as interfaces so we can mock them during testing
* Mocks (and related concepts of Fakes and Stubs) - Mocks are stand-ins for a class' normal dependencies, used only at test time.  They are capable of simulating interactions, returning values, and raising events, and can also cause a test to fail if their configured expectations are not met.

**OK, so how does it all fit together?**

Let's start with our abstract-ish CreditDecision component from before.  Let's imagine that the component needs to ask a remote web service for a credit decision (we're going to skip the backend part here though...)  We might expect the class to look something like this:

```c#
public class CreditDecision{
    public string MakeCreditDecision(int creditScore){
        var service = new CreditDecisionService();
        return service.GetCreditDecision(creditScore);
    }
}
```
**Houston, We Have a Problem...**

OK, technically this code might be testable, but we're having to call into the external CreditDecisionService every time we want to test it.  The reason this is a problem is that this code does not follow the Dependency Injection principle - Since it creates its own dependency (the CreditDecisionService), we cannot control what happens at test time.

We can take a step in the right direction by refactoring the class for dependency injection:

```c#
public class CreditDecision{

    CreditDecisionService creditDecisionService;
    public CreditDecision(CreditDecisionService creditDecisionService)
    {
        this.creditDecisionService = creditDecisionService;
    }

    public string MakeCreditDecision(int creditScore){
       return creditDecisionService.GetCreditDecision(creditScore);
    }
}
```

**Now we're at least injecting CreditDecisionService, so we're ready to go, right?**

Not quite...  We're still not programming to an abstraction just yet - the CreditDecision depends on the concrete CreditDecisionService type instead of an abstraction such as an interface or abstract class.  Because of this, we cannot inject a mock instance of CreditDecisionService (most mocking frameworks work with abstractions).

Let's update the code so we can finally start testing!
```c#
// This assumes that there is an existing ICreditDecisionService interface
// and that CreditDecisionService implements it.
public class CreditDecision{

    ICreditDecisionService creditDecisionService;
    public CreditDecision(ICreditDecisionService creditDecisionService)
    {
        this.creditDecisionService = creditDecisionService;
    }

    public string MakeCreditDecision(int creditScore){
       return creditDecisionService.GetCreditDecision(creditScore);
    }
}
```

**Now** we're ready to start cooking!

### Moq:  Skimming the Surface
Moq (pronounced either "Mock" or "Mock You", I stick with "Mock") is a framework that lets us simulate dependencies at test time and monitor how our system under test interacts with them under various circumstances.

In a nutshell, this is how it works:  
* In our test class, we create a Mock instance for each dependency that the system under test relies upon.
* We configure our mock's various expectations and tell it what values to return under what circumstances
* We inject those mock instances (see code below) when creating our system under test
* We execute the method we want to test
* We ask each Mock to verify that all of its expectations were met

For example, a test suite for our CreditDecision component above might look like:
```c#
namespace UnitTesting.GettingStarted.Tests
{
    [TestFixture]
    public class CreditDecisionTests
    {
        Mock<ICreditDecisionService>  mockCreditDecisionService;

        CreditDecision systemUnderTest;

        [TestCase(100, "Declined")]
        [TestCase(549, "Declined")]
        [TestCase(550, "Maybe")]
        [TestCase(674, "Maybe")]
        [TestCase(675, "We look forward to doing business with you!")]
        public void MakeCreditDecision_Always_ReturnsExpectedResult(int creditScore, string expectedResult)
        {
            mockCreditDecisionService = new Mock<ICreditDecisionService>(MockBehavior.Strict);
            mockCreditDecisionService.Setup(p => p.GetDecision(creditScore)).Returns(expectedResult);


            systemUnderTest = new CreditDecision(mockCreditDecisionService.Object);
            var result = systemUnderTest.MakeCreditDecision(creditScore);

            Assert.That(result, Is.EqualTo(expectedResult));

            mockCreditDecisionService.VerifyAll();
        }
    }
}
```

The test itself should look familiar, but there have been some changes...

Notice that we declare a variable of type `Mock<ICreditDecisionService>` - this will end up representing the ICreditDecisionService that the system under test depends upon.

Just inside our test method, you'll see us creating and configuring the mock:

```c#
mockCreditDecisionService = new Mock<ICreditDecisionService>(MockBehavior.Strict);
mockCreditDecisionService.Setup(p => p.GetDecision(creditScore)).Returns(expectedResult);
```

We'll cover the MockBehavior.Strict bit in a minute, but let's focus on that second line - this is where we configure the mock for **this** test.  In this case, we're telling the mock *"Hey Mock, if your GetDecision method is invoked with this specific number (creditScore), return this response (expectedResult).  If it gets invoked with any other number, fail the test immediately".* (that's part of MockBehavior.Strict)

Next up, we execute the MakeCreditDecision method just like we did before, the only remaining step is asking our Mock instance if all of its expectations were fulfilled using `mockCreditDecisionService.VerifyAll()`

> **Note - Mock Behaviors:** You'll notice that when I create the mock above, I'm specifying an argument of MockBehavior.Strict.  This is a Moq-specific concept, best explained as:
> * Loose mocks will not notify you if unexpected actions occur, including unexpected method arguments, events, and method invocations.  On a loose mock, if a property getter is not configured, the mock will simply return the property type's default value, leading to subtle and difficult-to-diagnose bugs.
> * Strict mocks are the tattletales of the mock world - unless everything goes as expected (as configured when setting up the mock), they will fail the test.
>
> In my own coding, I prefer to use Strict mocks because they require me to be very explicit about what I expect to occur when a method under test is invoked.
> 
> It's also important to note that both Loose and Strict mocks will behave the same way with VerifyAll() - In either case, if there are any unmet expectations on the mock the test will fail.

## Conclusion
This tutorial only scratched the very surface of Unit Testing with NUnit and Moq, but hopefully has provided you with some additional understanding and perspective on this challenging, but highly beneficial practice.

Future tutorials will cover more advanced topics including:
* Dynamic test cases withNUnit's TestCaseSource subsystem
* Complex Moq configurations including sequences and eventing