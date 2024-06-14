using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Octokit;

namespace Nymph.Plugin.GitHub;

public static class SearchCode
{
    private static async Task<Seq<AtomItem<Octokit.SearchCode>>> SearchAsync(string query, string user, string repo)
    {
        try
        {
            var client = new GitHubClient(new ProductHeaderValue("Nymph"));
            var request = new SearchCodeRequest(query, user, repo);

            var result = await client.Search.SearchCode(request);
            return Seq(result.Items.Take(5).Select(c => new AtomItem<Octokit.SearchCode>(c)));
        }
        catch (Exception e)
        {
            // ignore
        }
        
        return await Task.FromResult(Seq<AtomItem<Octokit.SearchCode>>());
    }
    
    public static FunctionItem<AtomItem<Repository>, FunctionItem<AtomItem<string>, AtomItem<Octokit.SearchCode>>> CreateSearchCodeItem()
    {
        return new FunctionItem<AtomItem<Repository>, FunctionItem<AtomItem<string>, AtomItem<Octokit.SearchCode>>>(
            repository => 
                Task.FromResult(Seq<FunctionItem<AtomItem<string>, AtomItem<Octokit.SearchCode>>>([
                    new FunctionItem<AtomItem<string>, AtomItem<Octokit.SearchCode>>(query => 
                        SearchAsync(query.Value, repository.Value.Owner.Login, repository.Value.Name))
                    {
                        Description = $"Search code in {repository.Value.FullName}",
                    }
                ])))
        {
            Description = "Search GitHub Code"
        };
    }
}