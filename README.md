To run the project do the following:
	npm install
	dotnet restore
	dotnet user-secrets set ConnectionStrings:Default "<Enter Your Connection String>"
	webpack --config webpack.config.vendor.js
	webpack
	dotnet ef database update
	dotnet watch run