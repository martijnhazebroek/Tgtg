using System.Drawing;
using Colorful;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Flow
{
    internal class PrintBannerStep
    {
        private readonly ConsolePrinter _console;

        public PrintBannerStep(ConsolePrinter console)
        {
            _console = console;
        }

        public void Execute()
        {
            _console.Write(@"                                                                           
                                                                                                        
                              `..---..`                                
                           `-------------`                             
                         `-----------------`                           
                        .-------------------.                          
       .-//++++//:-.`` -----------------------`  ``````````            
     -/+++++++++++++++//::---------------------```````````````         
   `/+++++++++++++++++++++/::-------------------```````````````        
   /+++++++++++++++++++++++++/:-----------------.```````````````       
  .++++++++++++++++++++++++++++/:----------------````````````````      
  -++++++++++++++++++++++++++++++/:--------------.```````````````      
  .++++++++++++++++++++++++++++++++/:------------.```````````````      
  `++++++++++++++++++++++++++++++++++:------------```````````````      
   :++++++++++++++++++++++++++++++++++/:----------``````````````       
   `+++++++++++++++++++++++++++++++++++/:---------``````````````       
    .+++++++++++++++++++++++++++++++++++/:--------`````````````        
     -+++++++++++++++++++++++++++++++++++/:-------````````````         
      -+++++++++++++++++++++++++++++++++++/------.```````````          
       .+++++++++++++++++++++++++++++++++++:-----```````````           
        `/+++++++++++++++++++++++++++++++++/----.`````````             
          -++++++++++++++++++++++++++++++++/----`````````              
           `:+++++++++++++++++++++++++++++++---.```````                
             `:+++++++++++++++++++++++++++++--.``````                  
               `-/+++++++++++++++++++++++++:-.`````                    
                  .:++++++++++++++++++++++/-````                       
                     .:/+++++++++++++++++:.``                          
                        `.-:/+++++++++/-`                              
                              ```````");

            _console.WriteLine();
            _console.WriteLine("Too Good To Go", Color.Aqua);
            _console.WriteLine();
        }
    }
}