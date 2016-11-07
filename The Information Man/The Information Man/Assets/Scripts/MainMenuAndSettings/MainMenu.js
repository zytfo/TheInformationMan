var Play = false;
var Settings = false;
var Quit = false;

var camera1 : Camera;
var camera2 : Camera;
var camera3 : Camera;

var Back = false;
var Low = false;
var Medium = false;
var High = false;
var Fantastic = false;
var AdvancedSettings = false;

function OnMouseEnter() {
    GetComponent.<Renderer>().material.color = Color.red;
}

function OnMouseExit() {
    GetComponent.<Renderer>().material.color = Color.white;
}

function OnMouseUp() {
    if (Play) {
        Application.LoadLevel("preview");
    }
    if (Settings) {
        camera1.enabled = false;
        camera2.enabled = true;
    }
    if (Quit == true) {
        Application.Quit();
    }

    //=============
    if (Back == true) {
        camera1.enabled = true;
        camera2.enabled = false;
        camera3.enabled = false;
    }
    if (Low == true) {
        QualitySettings.currentLevel = QualityLevel.Simple;
    }
    if (Medium == true) {
        QualitySettings.currentLevel = QualityLevel.Good;
    }
    if (High == true) {
        QualitySettings.currentLevel = QualityLevel.Beautiful;
    } 
    if (Fantastic == true) {
        QualitySettings.currentLevel = QualityLevel.Fantastic;
    }
    if (AdvancedSettings == true) {
         camera3.enabled = true;
         camera2.enabled = false;
    }
}