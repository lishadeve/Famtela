## Getting Started with Docker in Windows :rocket:

- Install Docker on Windows via `https://docs.docker.com/docker-for-windows/install/`
- Don't close Docker after it installs
- Install Git from https://git-scm.com/download/win.
- Open up Powershell on Windows and run the following (run the commands separately)
    - `cd\`
    - `dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p securePassword123`
    - `dotnet dev-certs https --trust`
    - `git clone https://github.com/dotnet-architecture/eShopOnContainers.git`
    - After the download is complete run `cd\eShopOnContainers\src`
- This will start pulling MSSQL Server Image from Docker Hub if you don't already have this image. It's around 500+ Mbs of download.
- Once that is done, dotnet SDKs and runtimes are downloaded, if not present already. That's almost 200+ more Mbs of download.
- PS If you find any issues while Docker installs the nuget packages, it is most likely that your ssl certificates are not installed properly. Apart from that I also added the `--disable-parallel` in the `Server\Dockerfile`to ensure network issues don't pop-up. You can remove this option to speed up the build process.
- That's almost everything. Once the containers are available, migrations are updated in the MSSQL DB, default data is seeded.
- Browse to https://localhost:5005/ to use Famtela !
