using System;
using DataInspector.DataAccess.DAL;
using Sample.Domain;
namespace Sample.Domain {
    public class Sample_Domain_Root : BaseDirectPropertyCallDAL<Root> {
        public Sample_Domain_Root() : base() {
            callChainDispatchMap.Add("rootnodeid", RootNodeId);
            callChainDispatchMap.Add("glorpchild.isenabled", GlorpChild_IsEnabled);
            callChainDispatchMap.Add("glorpchild.valuef", GlorpChild_ValueF);
            callChainDispatchMap.Add("glorpchild.name", GlorpChild_Name);
            callChainArrayDispatchMap.Add("derpchild.alltheflerbs[]", DerpChild_AllTheFlerbs);
            callChainArrayDispatchMap.Add("derpchild.alltheflerbs.subids[]", DerpChild_AllTheFlerbs_SubIds);
            callChainDispatchMap.Add("derpchild.alltheflerbs.flerbid", DerpChild_AllTheFlerbs_FlerbId);
            callChainDispatchMap.Add("derpchild.referentialglorp.isenabled", DerpChild_ReferentialGlorp_IsEnabled);
            callChainDispatchMap.Add("derpchild.referentialglorp.valuef", DerpChild_ReferentialGlorp_ValueF);
            callChainDispatchMap.Add("derpchild.referentialglorp.name", DerpChild_ReferentialGlorp_Name);
            callChainDispatchMap.Add("derpchild.id", DerpChild_Id);
            callChainDispatchMap.Add("derpchild.uuid", DerpChild_Uuid);
        }
        private object RootNodeId(object inputObject) {
            return (inputObject as Root).RootNodeId;
        }
        private object GlorpChild_IsEnabled(object inputObject) {
            return (inputObject as Root).GlorpChild.IsEnabled;
        }
        private object GlorpChild_ValueF(object inputObject) {
            return (inputObject as Root).GlorpChild.ValueF;
        }
        private object GlorpChild_Name(object inputObject) {
            return (inputObject as Root).GlorpChild.Name;
        }
        private object DerpChild_AllTheFlerbs(object inputObject, int index) {
            return (inputObject as Derp).DerpChild.AllTheFlerbs[index];
        }
        private object DerpChild_AllTheFlerbs_SubIds(object inputObject, int index) {
            return (inputObject as Derp).DerpChild.AllTheFlerbs.SubIds[index];
        }
        private object DerpChild_AllTheFlerbs_FlerbId(object inputObject) {
            return (inputObject as Root).DerpChild.AllTheFlerbs.FlerbId;
        }
        private object DerpChild_ReferentialGlorp_IsEnabled(object inputObject) {
            return (inputObject as Root).DerpChild.ReferentialGlorp.IsEnabled;
        }
        private object DerpChild_ReferentialGlorp_ValueF(object inputObject) {
            return (inputObject as Root).DerpChild.ReferentialGlorp.ValueF;
        }
        private object DerpChild_ReferentialGlorp_Name(object inputObject) {
            return (inputObject as Root).DerpChild.ReferentialGlorp.Name;
        }
        private object DerpChild_Id(object inputObject) {
            return (inputObject as Root).DerpChild.Id;
        }
        private object DerpChild_Uuid(object inputObject) {
            return (inputObject as Root).DerpChild.Uuid;
        }

    }
}
