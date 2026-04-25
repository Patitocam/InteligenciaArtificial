using UnityEditor;
using UnityEngine;

class Importrt : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        var importer = (ModelImporter)assetImporter;

        importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
        importer.materialLocation = ModelImporterMaterialLocation.External;
        importer.materialName = ModelImporterMaterialName.BasedOnTextureName;
        importer.materialSearch = ModelImporterMaterialSearch.RecursiveUp;
    }
}
