import { FormGroup } from '@angular/forms';

export interface ValidationError {
  key: string;
  value: any;
}

export interface PropertyValidation<T = any> {
  propName: string;
  propValue: T;
  validationMessages: ValidationError[];
}

export const getValidationErrors = (form: FormGroup) => {
  const validationsArr: PropertyValidation[] = [];

  Object.keys(form.controls).forEach((key) => {
    const item = form.get(key);

    if (item) {
      const controlErrors = item.errors;

      if (controlErrors) {
        const validationMessages: ValidationError[] = [];

        Object.keys(controlErrors).forEach((keyError) => {
          validationMessages.push({
            key: keyError,
            value: controlErrors[keyError],
          });
        });

        validationsArr.push({
          propName: key,
          propValue: item.value,
          validationMessages: validationMessages,
        });
      }
    }
  });

  return validationsArr;
};

export const getValidationErrorText = (validationError: ValidationError) => {
  let message: string;

  switch (validationError.key) {
    case 'required':
      message = 'is required but contains no value';
      break;
    default:
      message = 'contains an invalid value';
      break;
  }

  return message;
};

export const getValidationSummaryText = (validation: PropertyValidation) => {
  const errorTextArr = validation.validationMessages.map(
    getValidationErrorText
  );

  let errorSummaryText = errorTextArr.join('; ');
  errorSummaryText = `${validation.propName} ${errorSummaryText}.`;

  return errorSummaryText;
};

export const getValidationErrorsSummaryText = (
  validationsArr: PropertyValidation[],
  summaryCaption?: string | null | undefined
) => {
  const summaryText = [
    summaryCaption ??
      'Please correct the following errors before submitting this form',
    validationsArr.map(getValidationSummaryText).join(' '),
  ].join(': ');

  return summaryText;
};
