build:
	rm -rf dist/*
	dotnet publish src/Inueco.Cinema.csproj \
	  -p:PublishSingleFile=true \
	  --self-contained true \
	  --configuration Release \
	  --os linux \
	  --output dist/ \
	  && (cd app/ && npm install && npx vite build)

run_frontend:
	(cd app && vite build --watch)

run: build
	(cd dist && ./Inueco.Cinema)