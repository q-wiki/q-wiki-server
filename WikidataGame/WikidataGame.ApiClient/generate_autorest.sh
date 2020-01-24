# Unity IL2CPP is unable to work with GUIDs (no support for generic sharing of value types)
# https://github.com/aspnet/EntityFrameworkCore/issues/13099#issuecomment-521601249
sed -i 's/{"format":"uuid",/{/g; s/,"format":"uuid"//g' swagger.json
autorest --input-file=swagger.json --csharp --output-folder=. --namespace=WikidataGame --add-credentials=true