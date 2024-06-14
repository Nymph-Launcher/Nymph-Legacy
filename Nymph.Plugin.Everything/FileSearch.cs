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
    public static Task<Seq<AtomItem<FileInfo>>> Search(AtomItem<string> query)
    {
        return Task.FromResult(Seq(Everything.Search().Name.Contains(query.Value)
            .Take(50)
            .Select(i => new AtomItem<FileInfo>(new FileInfo(i.FileName, i.Path+ $"\\{i.FileName}")))));
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
                FileName = item.Value.Path ,
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