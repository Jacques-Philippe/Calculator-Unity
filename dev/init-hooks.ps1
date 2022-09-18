echo "Installing tools..."
dotnet tool restore
echo "Done."

echo "Initializing hooks..."

cp .\dev\hooks\pre-commit .\.git\hooks

echo "Done."