# encoding: utf-8
# Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
# Changes may cause incorrect behavior and will be lost if the code is
# regenerated.

require 'uri'
require 'cgi'
require 'date'
require 'json'
require 'base64'
require 'erb'
require 'securerandom'
require 'time'
require 'timeliness'
require 'faraday'
require 'faraday-cookie_jar'
require 'concurrent'
require 'ms_rest'
require 'generated/storage/module_definition'
require 'ms_rest_azure'

module storage
  autoload :StorageAccounts,                                    'generated/storage/storage_accounts.rb'
  autoload :UsageOperations,                                    'generated/storage/usage_operations.rb'
  autoload :StorageManagementClient,                            'generated/storage/storage_management_client.rb'

  module Models
    autoload :StorageAccountListResult,                           'generated/storage/models/storage_account_list_result.rb'
    autoload :StorageAccountCheckNameAvailabilityParameters,      'generated/storage/models/storage_account_check_name_availability_parameters.rb'
    autoload :StorageAccountUpdateParameters,                     'generated/storage/models/storage_account_update_parameters.rb'
    autoload :StorageAccountCreateParameters,                     'generated/storage/models/storage_account_create_parameters.rb'
    autoload :StorageAccountRegenerateKeyParameters,              'generated/storage/models/storage_account_regenerate_key_parameters.rb'
    autoload :CustomDomain,                                       'generated/storage/models/custom_domain.rb'
    autoload :UsageName,                                          'generated/storage/models/usage_name.rb'
    autoload :StorageAccountKeys,                                 'generated/storage/models/storage_account_keys.rb'
    autoload :Usage,                                              'generated/storage/models/usage.rb'
    autoload :Endpoints,                                          'generated/storage/models/endpoints.rb'
    autoload :UsageListResult,                                    'generated/storage/models/usage_list_result.rb'
    autoload :CheckNameAvailabilityResult,                        'generated/storage/models/check_name_availability_result.rb'
    autoload :StorageAccount,                                     'generated/storage/models/storage_account.rb'
    autoload :Reason,                                             'generated/storage/models/reason.rb'
    autoload :AccountType,                                        'generated/storage/models/account_type.rb'
    autoload :ProvisioningState,                                  'generated/storage/models/provisioning_state.rb'
    autoload :AccountStatus,                                      'generated/storage/models/account_status.rb'
    autoload :UsageUnit,                                          'generated/storage/models/usage_unit.rb'
  end
end
