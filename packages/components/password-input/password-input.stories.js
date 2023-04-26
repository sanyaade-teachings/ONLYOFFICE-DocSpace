import React, { useState, useEffect } from "react";
import PasswordInput from ".";
import TextInput from "../text-input";

export default {
  title: "Components/PasswordInput",
  component: PasswordInput,
  parameters: {
    docs: {
      description: {
        component: "Paging is used to navigate med content pages",
      },
    },
  },
  argTypes: {
    settingMinLength: { description: "Property for story" },
    settingsUpperCase: { description: "Property for story" },
    settingsDigits: { description: "Property for story" },
    settingsSpecSymbols: { description: "Property for story" },
  },
};

const Template = ({
  settingMinLength,
  settingsUpperCase,
  settingsDigits,
  settingsSpecSymbols,
  tooltipPasswordLength,
  onChange,
  onValidateInput,
  onCopyToClipboard,
  ...args
}) => {
  const [value, setValue] = useState("");
  const [fakeSettings, setFakSettings] = useState();

  useEffect(() => {
    setFakSettings({
      minLength: settingMinLength,
      upperCase: settingsUpperCase,
      digits: settingsDigits,
      specSymbols: settingsSpecSymbols,
    });
    setValue("");
  }, [
    settingMinLength,
    settingsUpperCase,
    settingsDigits,
    settingsSpecSymbols,
  ]);

  const onChangeHandler = (e) => {
    onChange(e.currentTarget.value);
    setValue(e.currentTarget.value);
  };

  const onValidateInputHandler = (e) => {
    onValidateInput(e);
  };

  return (
    <div style={{ height: "110px", display: "grid", gridGap: "24px" }}>
      <TextInput
        name="demoEmailInput"
        size="base"
        isDisabled={args.isDisabled}
        isReadOnly={true}
        scale={true}
        value="demo@gmail.com"
      />

      <PasswordInput
        {...args}
        inputValue={value}
        onChange={onChangeHandler}
        tooltipPasswordLength={`${tooltipPasswordLength} ${settingMinLength}`}
        passwordSettings={fakeSettings}
        onValidateInput={onValidateInputHandler}
        tooltipOffsetLeft={150}
      />
    </div>
  );
};

export const Default = Template.bind({});
Default.args = {
  isDisabled: false,
  settingMinLength: 6,
  settingsUpperCase: false,
  settingsDigits: false,
  settingsSpecSymbols: false,
  simpleView: false,
  inputName: "demoPasswordInput",
  emailInputName: "demoEmailInput",
  isDisableTooltip: false,
  isTextTooltipVisible: false,
  tooltipPasswordTitle: "Password must contain:",
  tooltipPasswordLength: "minimum length: ",
  tooltipPasswordDigits: "digits",
  tooltipPasswordCapital: "capital letters",
  tooltipPasswordSpecial: "special characters (!@#$%^&*)",
  generatorSpecial: "!@#$%^&*",
  placeholder: "password",
  maxLength: 30,
};
