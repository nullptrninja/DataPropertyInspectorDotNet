using System;
using DataInspector.DataAccess.DAL;
using Sample.Domain;

namespace Sample.Domain.DAL {
    public class Sample_Domain_Root : BaseDirectPropertyCallDAL<Root> {
        public Sample_Domain_Root() : base() {
            callChainDispatchMap.Add("rootnodeid", RootNodeId);
            callChainDispatchMap.Add("glorpchild.isenabled", GlorpChild_IsEnabled);
            callChainDispatchMap.Add("glorpchild.valuef", GlorpChild_ValueF);
            callChainDispatchMap.Add("glorpchild.name", GlorpChild_Name);
            callChainArrayDispatchMap.Add("derpchild.alltheflerbs[].subids[]", DerpChild_AllTheFlerbs_SubIds);
            callChainArrayDispatchMap.Add("derpchild.alltheflerbs[].flerbid", DerpChild_AllTheFlerbs_FlerbId);
            callChainDispatchMap.Add("derpchild.referentialglorp.isenabled", DerpChild_ReferentialGlorp_IsEnabled);
            callChainDispatchMap.Add("derpchild.referentialglorp.valuef", DerpChild_ReferentialGlorp_ValueF);
            callChainDispatchMap.Add("derpchild.referentialglorp.name", DerpChild_ReferentialGlorp_Name);
            callChainDispatchMap.Add("derpchild.id", DerpChild_Id);
            callChainDispatchMap.Add("derpchild.uuid", DerpChild_Uuid);
        }
        private object RootNodeId(Root inputObject) {
            return inputObject.RootNodeId;
        }
        private object GlorpChild_IsEnabled(Root inputObject) {
            return inputObject.GlorpChild.IsEnabled;
        }
        private object GlorpChild_ValueF(Root inputObject) {
            return inputObject.GlorpChild.ValueF;
        }
        private object GlorpChild_Name(Root inputObject) {
            return inputObject.GlorpChild.Name;
        }
        private object DerpChild_AllTheFlerbs_SubIds(Root inputObject, int[] indicies) {
            return inputObject.DerpChild.AllTheFlerbs[indicies[0]].SubIds[indicies[1]];
        }
        private object DerpChild_AllTheFlerbs_FlerbId(Root inputObject, int[] indicies) {
            return inputObject.DerpChild.AllTheFlerbs[indicies[0]].FlerbId;
        }
        private object DerpChild_ReferentialGlorp_IsEnabled(Root inputObject) {
            return inputObject.DerpChild.ReferentialGlorp.IsEnabled;
        }
        private object DerpChild_ReferentialGlorp_ValueF(Root inputObject) {
            return inputObject.DerpChild.ReferentialGlorp.ValueF;
        }
        private object DerpChild_ReferentialGlorp_Name(Root inputObject) {
            return inputObject.DerpChild.ReferentialGlorp.Name;
        }
        private object DerpChild_Id(Root inputObject) {
            return inputObject.DerpChild.Id;
        }
        private object DerpChild_Uuid(Root inputObject) {
            return inputObject.DerpChild.Uuid;
        }

    }
}
