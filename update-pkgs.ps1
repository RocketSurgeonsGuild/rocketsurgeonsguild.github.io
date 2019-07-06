param($token)

$repos = Invoke-RestMethod -Uri "https://api.github.com/orgs/RocketSurgeonsGuild/repos" -Headers @{ Authorization = "Bearer $token" } -ResponseHeadersVariable 'ResponseHeaders'

do {
    $link = $null;
    try {
        $links = $ResponseHeaders.Link -split ',';
        $links
        if ($links[0] -match 'next') {
            $link = ($links[0] -split ';')[0].Trim('<>');
        }
    }
    catch {

    }
    if ($link) {
        $repos += Invoke-RestMethod -Uri $link -Headers @{ Authorization = "Bearer $token" } -ResponseHeadersVariable 'ResponseHeaders'
    }
}
while ($link);

foreach ($repo in $repos) {
    # mkdir
    $repo.full_name
    # $repo

    # $dir = "./pkgs/$($repo.name)".ToLowerInvariant();
    $dir = "./pkgs/"
    # $repo
    try {
        $srcContents = Invoke-RestMethod -Uri "https://api.github.com/repos/$($repo.full_name)/contents/src" -Headers @{ Authorization = "Bearer $token" };
        foreach ($item in $srcContents | where { $_.type -eq 'dir' }) {
            $projectContents = Invoke-RestMethod -Uri "https://api.github.com/repos/$($repo.full_name)/contents/$($item.path)" -Headers @{ Authorization = "Bearer $token" };
            $project = $projectContents | where { $_.name.EndsWith(".csproj") } | select -First 1

            $projectName = $project.name.Substring(0, $project.name.Length - ".csproj".Length);
            $projectNameLower = $projectName.ToLowerInvariant();

            if (-not (Test-Path $dir)) {
                mkdir $dir;
            }

            if ($projectName -match '.Abstractions') {
                continue;
            }

            if (-not (Test-Path "$dir/$($projectNameLower).yml")) {
                Set-Content "$dir/$($projectNameLower).yml" "Name: $projectName
NuGet: $projectName
Assemblies:
- `"/**/$projectName.dll`"
Repository: $($repo.html_url)
Author: Rocket Surgeons
Description: `"TODO`"
Categories:
- Cake"
            }
        }
    }
    catch {
        Write-Error $_
    }
}
