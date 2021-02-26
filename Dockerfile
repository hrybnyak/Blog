FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /Blog
COPY . /Blog
COPY . /DAL
COPY . /BLL
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 8000/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh