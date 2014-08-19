using MugenInjection.Infrastructure;

namespace Inconspicuous.Framework {
	public static class MugenCompilerHints {
		public static void Hint() {
			var dict = new DictionarySettingsImpl();
			dict.Get<bool>("test"); bool dummyBool; dict.TryGet<bool>("test", out dummyBool);
			dict.Get<int>("test"); int dummyInt; dict.TryGet<int>("test", out dummyInt);
			dict.Get<BindingPriority>("test"); BindingPriority dummyBindingPriority; dict.TryGet<BindingPriority>("test", out dummyBindingPriority);
			dict.Get<ServiceType>("test"); ServiceType dummyServiceType; dict.TryGet<ServiceType>("test", out dummyServiceType);
			dict.Get<TypeChangedAction>("test"); TypeChangedAction dummyTypeChangedAction; dict.TryGet<TypeChangedAction>("test", out dummyTypeChangedAction);
			dict.Get<ConverterResult>("test"); ConverterResult dummyConverterResult; dict.TryGet<ConverterResult>("test", out dummyConverterResult);
			dict.Get<ActivateType>("test"); ActivateType dummyActivateType; dict.TryGet<ActivateType>("test", out dummyActivateType);
		}
	}
}
