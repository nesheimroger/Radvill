Radvill
=======

Programs:

Visual Studio 2012
 - Extensions
    - Mindscape Web Workbench
    - Nuget Package Manager
    
SQL Express 2012 with tools (http://www.microsoft.com/en-us/download/details.aspx?id=29062)
 - Instance Name : SQLEXPRESS
 - Set up mixed mode authentication (password utv+01)
 
IIS:
 - http://www.iis.net/learn/install/installing-iis-7/installing-iis-on-windows-vista-and-windows-7
 - Basic authentication is needed

Git: 
 - http://git-scm.com/



Setup: 
 - git clone [REPOSITORY]
 - Open Radvill.sln
 - Enable Restore packages on build
 - Build (ctrl - shift - B)
 - Close Visual Studio (due to bug with nuget packages)
 - Open Radvill.Sln
 - Nuget Package Manager Console
    - Update-Database
 - ISS
    - Add Website
        - Hostname: Radvill.WebFront
        - Directory: Radvill/Radvill.WebFront
        - Set Application pool to 4.0
    - Add Website
        - Hostname: Radvill.WebApi
        - Directory: Radvill/Radvill.WebApi
        - Set Application pool to 4.0
 - Hosts (Windows/System32/Drivers/etc/host) 
        - add: 127.0.0.1 Radvill.WebFront
        - add: 127.0.0.1 Radvill.WebApi
