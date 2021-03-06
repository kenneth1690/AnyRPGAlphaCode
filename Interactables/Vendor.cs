using AnyRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnyRPG {
    public class Vendor : InteractableOption {

        public override event System.Action<IInteractable> MiniMapStatusUpdateHandler = delegate { };

        public override Sprite MyIcon { get => (SystemConfigurationManager.MyInstance.MyVendorInteractionPanelImage != null ? SystemConfigurationManager.MyInstance.MyVendorInteractionPanelImage : base.MyIcon); }
        public override Sprite MyNamePlateImage { get => (SystemConfigurationManager.MyInstance.MyVendorNamePlateImage != null ? SystemConfigurationManager.MyInstance.MyVendorNamePlateImage : base.MyNamePlateImage); }

        [SerializeField]
        private VendorItem[] items;

        [SerializeField]
        private List<VendorCollection> vendorCollections = new List<VendorCollection>();

        [SerializeField]
        private List<string> vendorCollectionNames = new List<string>();

        protected override void Start() {
            base.Start();
            interactionPanelTitle = "Purchase Items";
        }

        public void InitWindow(ICloseableWindowContents vendorUI) {
            (vendorUI as VendorUI).PopulateDropDownList(vendorCollections);
        }

        public override bool Interact(CharacterUnit source) {
            base.Interact(source);
            //Debug.Log(source + " attempting to interact with " + gameObject.name);
            if (!PopupWindowManager.MyInstance.vendorWindow.IsOpen) {
                //Debug.Log(source + " interacting with " + gameObject.name);
                PopupWindowManager.MyInstance.vendorWindow.MyCloseableWindowContents.OnOpenWindow += InitWindow;
                PopupWindowManager.MyInstance.vendorWindow.OpenWindow();
                return true;
            }
            return false;
        }

        public override void StopInteract() {
            base.StopInteract();
            PopupWindowManager.MyInstance.vendorWindow.CloseWindow();
            PopupWindowManager.MyInstance.vendorWindow.MyCloseableWindowContents.OnOpenWindow -= InitWindow;
        }

        public override void HandlePrerequisiteUpdates() {
            base.HandlePrerequisiteUpdates();
            MiniMapStatusUpdateHandler(this);
        }

        public override void SetupScriptableObjects() {
            base.SetupScriptableObjects();

            if (vendorCollectionNames != null && vendorCollectionNames.Count > 0) {
                vendorCollections = new List<VendorCollection>();
                foreach (string vendorCollectionName in vendorCollectionNames) {
                    VendorCollection tmpVendorCollection = SystemVendorCollectionManager.MyInstance.GetResource(vendorCollectionName);
                    if (tmpVendorCollection != null) {
                        vendorCollections.Add(tmpVendorCollection);
                    } else {
                        Debug.LogError("Quest.SetupScriptableObjects(): Could not find vendor collection : " + vendorCollectionName + " while inititalizing " + MyName + ".  CHECK INSPECTOR");
                    }
                }
            }

        }


    }

}