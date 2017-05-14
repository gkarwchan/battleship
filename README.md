# Battleship information.

For Unit Test:
--------
For unit test, I used NSpec unit test framework, and moq for mocking.
When you build the project, nuget should download these libraries.
Nspec has a console runner, to run it, I added a batch file called “runTests.bat”
This batch file will run all the tests and show them to you on the console.

P.S: in case you get an error when you run the unit tests: “This app cannot run on your PC”,
Then from the Visual studio package manager console, just issue again:

```shell
Install-package nspec
```

And this will fix it.

