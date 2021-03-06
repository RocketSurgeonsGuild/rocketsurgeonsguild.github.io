Description: What is a "Convention"?
Order: 100
---

# Why do we think conventions are helpful?

| general agreement or consent; accepted usage, especially as a standard of procedure.

When ASP.NET Core was released we gained the ability for more fine grained control how our applications are configured and setup.  This allows us the ability to control how our applications work, but with that comes the responsibility to ensure that we configure everything properly. This can cause the configuration code to quickly turn into a mess if you're not careful.

* What if you need to configure a different database for development vs production?
* What assemblies do you need to load to capture all your controllers?

Now please keep in mind we here at the Rocket Surgeons Guild completely understand that _all of what we provide_ can be done purely with extension methods, custom code and so on.  What we offer with conventions is the ability to make your life just a little bit easier, so you can forget about the glue code and just worry about working on your awesome application.

# What is a convention?
A convention is something very similar to something like an [Autofac Module][1] or [Owin Startup Attribute][2].  In essence it is just a a piece of code that will run a given time.  However we try and make it easier to bring your configuration with you using the new [`Microsoft.Extensions` libraries provided by Microsoft][ME].

These conventions will be run during specific life cycle events of application startup.  For example we have conventions for [Dependency Injection](/conventions/service/), [Configuration](/conventions/configuration/) and even [Command Line](/conventions/commandLine/).

While your convention is registering you get access a [Convention Context](/api/Rocket.Surgery.Conventions/IConventionContext/).  This context has properties that allow you to make intelligent decisions about what services to register, logging providers to add or command line operations are available.  The properties vary by convention, for example the [Command Line Convention](/api/Rocket.Surgery.Extensions.CommandLine/ICommandLineConventionContext/) does not expose the [`IRocketEnvironment`](/api/Rocket.Surgery.Conventions/IRocketEnvironment/) because the outcome of the execution of the command line could affect the environment.



[1]: https://autofaccn.readthedocs.io/en/latest/configuration/modules.html
[2]: https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-startup-class-detection
[ME]: https://github.com/aspnet/Extensions