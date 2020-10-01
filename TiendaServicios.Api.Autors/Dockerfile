#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Docker file -> Este archivo contiene todas la descripcion de las  tareas que  va a realizar en funcion a crear la imagen del micropservice

#Va a descargar una imagen de las libreria del asp.net core 3.1 este es el comando que se encargara de descargar las librerias 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# y ubicarla en la carpeta app de tu conteiner local en tu computadora
WORKDIR /app
#a su vez esponer esos ervicios en el puerto 80
EXPOSE 80

#Lo que hace todo este parrafo es consumir los rercusos que se encuentras en estas librerias .net core 3.1 y tambien consumir el codigo del proyecto, copiar el codigo y comenzar el proceso de restore, compile y de build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TiendaServicios.Api.Autors/TiendaServicios.Api.Autors.csproj", "TiendaServicios.Api.Autors/"]
RUN dotnet restore "TiendaServicios.Api.Autors/TiendaServicios.Api.Autors.csproj"
COPY . .
WORKDIR "/src/TiendaServicios.Api.Autors"
RUN dotnet build "TiendaServicios.Api.Autors.csproj" -c Release -o /app/build

#Si el proceso anterior fue correcto, creara un archivo con binarios en una carpeta publish 
FROM build AS publish
RUN dotnet publish "TiendaServicios.Api.Autors.csproj" -c Release -o /app/publish

# y como parte final va a crear estas dll
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TiendaServicios.Api.Autors.dll"]

#al terminar esto se habra creado la imagen de mi proyecto conteniendo las libreria de soporte de .net core y 
#tambien el codigo del proyecto de este modo al ejecutar, al ejecutar el container de la imagen va a ser levantar el servicios de mi microservices 
