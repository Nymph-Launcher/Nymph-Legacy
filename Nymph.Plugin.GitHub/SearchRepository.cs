using System.Diagnostics;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Octokit;

namespace Nymph.Plugin.GitHub;

public static class SearchRepository
{
    private static async Task<Seq<AtomItem<Repository>>> SearchAsync(string query)
    {
        try
        {
            var client = new GitHubClient(new ProductHeaderValue("Nymph"));
            var request = new SearchRepositoriesRequest(query);

            var result = await client.Search.SearchRepo(request);

            return Seq(result.Items.Take(5).Select(r => new AtomItem<Repository>(r)));
        }
        catch (Exception e)
        {
            // ignored
        }

        return await Task.FromResult(Seq<AtomItem<Repository>>());
    }
    
    public static FunctionItem<AtomItem<string>, AtomItem<Repository>> CreateSearchRepositoryItem()
    {
        return new FunctionItem<AtomItem<string>, AtomItem<Repository>>(async query =>
        {
            var repositories = await SearchAsync(query.Value);
            return repositories;
        })
        {
            Description = "Search GitHub Repositories"
        };
    }
    
    public static FunctionItem<AtomItem<Repository>, AtomItem<Repository>> CreateOpenRepositoryItem()
    {
        return new FunctionItem<AtomItem<Repository>, AtomItem<Repository>>(repository =>
        {
            var url = repository.Value.HtmlUrl;
            if (url is not null)
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            }
            return Task.FromResult(Seq([repository]));
        })
        {
            Description = "Open GitHub Repository"
        };
    }
}