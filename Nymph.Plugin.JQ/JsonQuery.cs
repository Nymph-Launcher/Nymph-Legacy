using System.Text.Json.Nodes;
using Coeus.Functions;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nymph.Model.Item;
using static LanguageExt.Prelude;
using Coeus.Functions;
using Coeus.Results;
using Coeus;
using Sprache;

namespace Nymph.Plugin.JQ;

using UpdateJsonFunctionItem = FunctionItem<AtomItem<JToken>, FunctionItem<AtomItem<string>, AtomItem<JToken>>>;

public static class JsonQuery
{
    private static Task<Seq<AtomItem<JToken>>> ParseJsonItem(AtomItem<string> jsonString)
    {
        try
        {
            var json = JToken.Parse(jsonString.Value);
            return Task.FromResult(Seq([new AtomItem<JToken>(json)]));
        }
        catch (JsonReaderException _)
        {
            return Task.FromResult(Seq<AtomItem<JToken>>());
        }
    }
    
    public static FunctionItem<AtomItem<string>, AtomItem<JToken>> CreateParseJsonItem()
    {
        return new FunctionItem<AtomItem<string>, AtomItem<JToken>>(ParseJsonItem)
        {
            Description = "Parse JSON"
        };
    }

    public static UpdateJsonFunctionItem CreateUpdateJsonItem()
    {
        return new UpdateJsonFunctionItem(item =>
        {
            var json = item.Value;
            return Task.FromResult(Seq([new FunctionItem<AtomItem<string>, AtomItem<JToken>>(query =>
            {
                try
                {
                    var output = query.Value.EvalToToken(json);
                    return Task.FromResult(Seq([new AtomItem<JToken>(output) ?? item]));
                }
                catch (Exception _)
                {
                    return Task.FromResult(Seq([item]));
                }
                
            })
            {
                Description = "Update JSON with provided JQ query"
            }]));
        })
        {
            Description = "Query JSON with JQ"
        };
    }

    public static FunctionItem<AtomItem<JToken>, AtomItem<string>> CreateJTokenToStringItem()
    {
        return new FunctionItem<AtomItem<JToken>, AtomItem<string>>(item => Task.FromResult(Seq([new AtomItem<string>(item.Value.ToString())])));
    }
    
    
}