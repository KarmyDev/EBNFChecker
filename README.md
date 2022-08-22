# EBNFChecker
An EBNF Checker written in C# using Eto.Parser and Iso14977 style

## Why does this exists?
Well, simple explanation - there just wasn't any good EBNF Checker so I wrote my own, so don't judge my code for what it is :P

# How do I build it?
You need to have dotnet and git installed,
If you're using Linux and just happen to have `apt` then all you have to do is to type `sudo apt install dotnet` to install dotnet.

First go to your Projects/ folder and type `git clone https://github.com/KarmyDev/EBNFChecker`, and you should see EBNFChecker folder after it finishes downloading.
Go to it via terminal (`cd EBNFChecker/`) and type `dotnet build` in order to build it.
Everything should be in `./bin/Debug/net5.0/` folder.

# How do I use it?
If you go to the bin folder (`cd ./bin/Debug/net5.0/`) you should get Usage and a lot of errors after you type `./ebnfcheck`, thats normal.
It will tell you that you have to provide EBNF source, and TEST file to check if the syntax you wrote is correct.

Usage: `./ebnfcheck <EBNF SOURCE> <TEST FILE> <START SEQUENCE KEYWORD>`

Example: `./ebnfcheck ./MyFile.ebnf ./Test.txt startSequence`

# How do I know if the test was succesfull?
First at the bottom there is a `Success: ` indicator that tells you if test went successfully.
If it says `True` then it went alright, if it says `False` then something is wrong, Check `ErrorMsg:` line.

Also if you notice, there should be my small pointer that shows you where the problem is. It's a nice little touch from me <3

Feel free to use it!
