using System.Diagnostics;
using System.IO;
using System.Windows;
using EverythingNet.Interfaces;
using LanguageExt;
using Nymph.Model.Item;
using static LanguageExt.Prelude;
using EverythingClient = EverythingNet.Core.Everything;

namespace Nymph.Plugin.Everything;

public record FileInfo(string Name, string Path);

public static class FileSearch
{
    private static readonly IEverything Everything = new EverythingClient();
    private static async Task<Seq<AtomItem<FileInfo>>> Search(AtomItem<string> query)
    {
        return await Task.FromResult(Seq(Everything.Search().Name.Contains(query.Value)
            .Take(50)
            .Select(i => new AtomItem<FileInfo>(new FileInfo(i.FileName, i.Path)))));
    }

    private static async Task<Seq<AtomItem<FileInfo>>> GetFiles(string path, string pattern)
    {
        var result = Seq<AtomItem<FileInfo>>([]);
        if (!Directory.Exists(path)) return result;
        var filePaths = Directory.GetFiles(path, pattern);
        var dirPaths = Directory.GetDirectories(path, pattern);
        return await Task.FromResult(result
            .Concat(filePaths.Select(p => new AtomItem<FileInfo>(new FileInfo(Path.GetFileName(p), path))))
            .Concat(dirPaths.Select(p => new AtomItem<FileInfo>(new FileInfo(Path.GetFileName(p), path)))));

    }
    
    public static FunctionItem<AtomItem<FileInfo>, FunctionItem<AtomItem<string>, AtomItem<FileInfo>>> CreateFileSearchItem()
    {
        return new FunctionItem<AtomItem<FileInfo>, FunctionItem<AtomItem<string>, AtomItem<FileInfo>>>(path =>
            Task.FromResult(Seq<FunctionItem<AtomItem<string>, AtomItem<FileInfo>>>([
                new FunctionItem<AtomItem<string>, AtomItem<FileInfo>>(
                    async pattern => await GetFiles(path.Value.Path, pattern.Value))
                {
                    Description = $"Search file under {path.Value.Name}"
                }
            ])))
        {
            Description = "Search file under dir"
        };
    }
    
    
    
    public static FunctionItem<AtomItem<string>, AtomItem<FileInfo>> CreateEverythingSearchItem()
    {
        return new FunctionItem<AtomItem<string>, AtomItem<FileInfo>>(Search){Description = "Everything File Search"};
    }
    
    public static FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>> CreateOpenFileItem()
    {
        return new FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>(item =>
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = item.Value.Path + $"\\{item.Value.Name}",
                UseShellExecute = true
            });
            return Task.FromResult(Seq([item]));
        })
        {
            Description = "Open the File"
        };
    }

    public static FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>> CreateOpenDirItem()
    {
        return new FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>(item =>
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = item.Value.Path,
                UseShellExecute = true
            });
            return Task.FromResult(Seq([item]));
        })
        {
            Description = "Open the File's Directory"
        };
    }
}