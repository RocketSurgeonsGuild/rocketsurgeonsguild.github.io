<Query Kind="Statements">
  <NuGetReference>Buildalyzer</NuGetReference>
  <NuGetReference>Octokit</NuGetReference>
  <NuGetReference>Octokit.Reactive</NuGetReference>
  <NuGetReference>System.Reactive</NuGetReference>
  <NuGetReference>System.Reactive.Core</NuGetReference>
  <NuGetReference>System.Reactive.Interfaces</NuGetReference>
  <NuGetReference>System.Reactive.Linq</NuGetReference>
  <NuGetReference>System.Reactive.PlatformServices</NuGetReference>
  <Namespace>Buildalyzer</Namespace>
  <Namespace>Octokit</Namespace>
  <Namespace>Octokit.Clients</Namespace>
  <Namespace>Octokit.Internal</Namespace>
  <Namespace>Octokit.Reactive</Namespace>
  <Namespace>Octokit.Reactive.Clients</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
</Query>

var client = new ObservableGitHubClient(new ProductHeaderValue("internaltooling.to.update.repo"), new InMemoryCredentialStore(new Credentials("e85bca949ed9979c1e2f7043fb9db8b3644f91b3")));

var repos = client.Repository.GetAllForOrg("RocketSurgeonsGuild");

var tempPath = @"D:\Development\ReadySelect\rocketsurgery\rocketsurgeonsguild.github.io\.tmp";

var clonedRepos = repos
	.Where(repo => !repo.Archived)
	.Select(repo =>
{
	var path = Path.Combine(tempPath, repo.Name);
	Process.Start(new ProcessStartInfo("git", $"clone --depth 1 --single-branch {repo.CloneUrl} {path }")
	{
		CreateNoWindow = true
	}).WaitForExit();
	return (path, repo);
});

var solutions = clonedRepos.SelectMany((x =>
{
	var (path, repo) = x;

	return Directory.EnumerateFiles(path, "*.sln")
		.ToObservable()
		.Select(solutionFilePath => (path, repo, solutionFilePath));
}));

var projects = solutions
	.Select(x =>
	{
		var analyzerManager = new AnalyzerManager(x.solutionFilePath);
		return (x.path, x.repo, x.solutionFilePath, analyzerManager);
	})
	.SelectMany(x =>
	{
		var (path, repo, solutionFilePath, analyzerManager) = x;
		return analyzerManager.Projects
			.Where(z => z.Key.Contains("/src/") || z.Key.Contains(@"\src\"))
			
			//.Where(x => x.Key.Contains("Essentials"))
			.Select(project =>
			{
				return (path, repo, solutionFilePath, analyzerManager, projectFilePath: project.Key, project: project.Value, projectBuild: project.Value.Build());
			})
			.ToObservable();
	})
	.Distinct(x => x.projectFilePath);

await projects
.SubscribeOn(NewThreadScheduler.Default)
.ForEachAsync(x =>
{
	x.projectBuild.First().GetProperty("AssemblyName").Dump();
});