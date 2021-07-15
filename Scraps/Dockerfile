FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY bin/Release/net5.0/ App/
WORKDIR /App
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Scraps.dll"]