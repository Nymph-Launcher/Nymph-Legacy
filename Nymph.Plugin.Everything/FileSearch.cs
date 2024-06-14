using System.Diagnostics;
using System.IO;
using System.Windows;
using EverythingNet.Interfaces;
using LanguageExt;
using Nymph.Model.Item;
using static LanguageExt.Prelude;
using EverythingClient = EverythingNet.Core.Everything;

namespace Nymph.Plugin.Everything;

public record FileInfo(string Name);

public static class FileSearch
{
    private static readonly IEverything Everything = new EverythingClient();
    public static Task<Seq<FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>>> Search(AtomItem<string> query)
    {
        return Task.FromResult(Seq(Everything.Search().Name.Contains(query.Value)
            .Take(50)
            .Select(i =>
            {
                var func =  new FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>(result => Task.Run(
                    () =>
                    {
                        if (File.Exists(i.Path))
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo(i.Path) { UseShellExecute = true });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error opening file: " + ex.Message);
                            }
                        }

                        return Seq([result]);
                    }
                ))
                {
                    Description = i.FileName
                };
                return func;
            })));
    }
    
    public static FunctionItem<AtomItem<string>, FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>> CreateEverythingSearchItem()
    {
        return new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<FileInfo>, AtomItem<FileInfo>>>(Search);
    }
}