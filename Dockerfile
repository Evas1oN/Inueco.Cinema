FROM mcr.microsoft.com/dotnet/runtime:8.0
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENV DB_PATH=/var/db.sqlite
WORKDIR /app
COPY dist/ .
RUN chmod 755 ./Inueco.Cinema
ENTRYPOINT [ "./Inueco.Cinema" ]