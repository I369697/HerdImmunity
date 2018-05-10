using System;
using Immunization.Classes;
using System.IO;
using HerdImmunity.Classes;
using IniParser;
using IniParser.Model;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Immunization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonBegin_Click(object sender, EventArgs e)
        {
            PopulationProperties population = new PopulationProperties(Convert.ToInt16(numericUpDownPopulationSize.Value),
                Convert.ToDouble(numericUpDownPopulationVaccinationRate.Value), Convert.ToDouble(numericUpDownPopulationImmunodeficiency.Value));

            VaccineProperties vaccination = new VaccineProperties(Convert.ToDouble(numericUpDownVaccineEfficacyRateMin.Value),
                Convert.ToDouble(numericUpDownVaccineEfficacyRateMax.Value), Convert.ToDouble(numericUpDownVaccineMortalityRateMin.Value),
                Convert.ToDouble(numericUpDownVaccineMortalityRateMax.Value));

            var scene = new Scene(population, getDisease(), vaccination);

            // Start simulation in a new thread - otherwise the UI may stop responding
            Task.Factory.StartNew(() =>
            {
                new SimulationWindow(scene);
            });
        }

        private void buttonPopulationSave_Click(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.ThrowExceptionsOnError = false;
            IniData data = new IniData();

            data["Sample"]["Size"] = numericUpDownPopulationSize.Text;
            data["Sample"]["VaccinationRate"] = Utility.FromNumericControlValue(numericUpDownPopulationVaccinationRate.Value);
            data["Sample"]["Immunodeficiency"] = Utility.FromNumericControlValue(numericUpDownPopulationImmunodeficiency.Value);

            parser.WriteFile($"{Constants.INIPopulationsPath}/{comboBoxPopulation.Text}.ini", data);

            setStatus($"Saved '{comboBoxPopulation.Text}' to file.");

            RefreshComboBoxes();
        }

        private void comboBoxPopulation_SelectedIndexChanged(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile($"{Constants.INIPopulationsPath}/{comboBoxPopulation.Text}.ini");

            numericUpDownPopulationSize.Value = Utility.ToNumericControlValue(data["Sample"]["Size"]);
            numericUpDownPopulationVaccinationRate.Value = Utility.ToNumericControlValue(data["Sample"]["VaccinationRate"]);
            numericUpDownPopulationImmunodeficiency.Value = Utility.ToNumericControlValue(data["Sample"]["Immunodeficiency"]);

            if (comboBoxPopulation.Focused)
                setStatus($"Loaded '{comboBoxPopulation.Text}' from file.");
        }

        private void buttonDiseaseSave_Click(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = new IniData();

            data["Disease"]["MortalityRateMin"] = Utility.FromNumericControlValue(numericUpDownDiseaseMortalityRateMin.Value); 
            data["Disease"]["MortalityRateMax"] = Utility.FromNumericControlValue(numericUpDownDiseaseMortalityRateMax.Value);
            data["Disease"]["InfectionRateMin"] = Utility.FromNumericControlValue(numericUpDownDiseaseInfectionRateMin.Value);
            data["Disease"]["InfectionRateMax"] = Utility.FromNumericControlValue(numericUpDownDiseaseInfectionRateMax.Value);
            data["Disease"]["IncubationPeriodMin"] = Utility.FromNumericControlValue(numericUpDownDiseaseIncubationPeriodMin.Value);
            data["Disease"]["IncubationPeriodMax"] = Utility.FromNumericControlValue(numericUpDownDiseaseIncubationPeriodMax.Value);
            data["Disease"]["LatencyPeriodMin"] = Utility.FromNumericControlValue(numericUpDownDiseaseLatencyPeriodMin.Value);
            data["Disease"]["LatencyPeriodMax"] = Utility.FromNumericControlValue(numericUpDownDiseaseLatencyPeriodMax.Value);
            data["Disease"]["TerminationPeriodMin"] = Utility.FromNumericControlValue(numericUpDownDiseaseTerminationPeriodMin.Value);
            data["Disease"]["TerminationPeriodMax"] = Utility.FromNumericControlValue(numericUpDownDiseaseTerminationPeriodMax.Value);

            data["Vaccine"]["MortalityRateMin"] = Utility.FromNumericControlValue(numericUpDownVaccineMortalityRateMin.Value);
            data["Vaccine"]["MortalityRateMax"] = Utility.FromNumericControlValue(numericUpDownVaccineMortalityRateMax.Value);
            data["Vaccine"]["EfficacyRateMin"] = Utility.FromNumericControlValue(numericUpDownVaccineEfficacyRateMin.Value);
            data["Vaccine"]["EfficacyRateMax"] = Utility.FromNumericControlValue(numericUpDownVaccineEfficacyRateMax.Value);

            parser.WriteFile($"{Constants.INIDiseasesPath}/{comboBoxDisease.Text}.ini", data);

            setStatus($"Saved '{comboBoxDisease.Text}' to file.");

            RefreshComboBoxes();
        }

        private void comboBoxDisease_SelectedIndexChanged(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile($"{Constants.INIDiseasesPath}/{comboBoxDisease.Text}.ini");

            numericUpDownDiseaseMortalityRateMin.Value = Utility.ToNumericControlValue(data["Disease"]["MortalityRateMin"]);
            numericUpDownDiseaseMortalityRateMax.Value = Utility.ToNumericControlValue(data["Disease"]["MortalityRateMax"]);
            numericUpDownDiseaseInfectionRateMin.Value = Utility.ToNumericControlValue(data["Disease"]["InfectionRateMin"]);
            numericUpDownDiseaseInfectionRateMax.Value = Utility.ToNumericControlValue(data["Disease"]["InfectionRateMax"]);
            numericUpDownDiseaseIncubationPeriodMin.Value = Utility.ToNumericControlValue(data["Disease"]["IncubationPeriodMin"]);
            numericUpDownDiseaseIncubationPeriodMax.Value = Utility.ToNumericControlValue(data["Disease"]["IncubationPeriodMax"]);
            numericUpDownDiseaseLatencyPeriodMin.Value = Utility.ToNumericControlValue(data["Disease"]["LatencyPeriodMin"]);
            numericUpDownDiseaseLatencyPeriodMax.Value = Utility.ToNumericControlValue(data["Disease"]["LatencyPeriodMax"]);
            numericUpDownDiseaseTerminationPeriodMin.Value = Utility.ToNumericControlValue(data["Disease"]["TerminationPeriodMin"]);
            numericUpDownDiseaseTerminationPeriodMax.Value = Utility.ToNumericControlValue(data["Disease"]["TerminationPeriodMax"]);

            numericUpDownVaccineMortalityRateMin.Value = Utility.ToNumericControlValue(data["Vaccine"]["MortalityRateMin"]);
            numericUpDownVaccineMortalityRateMax.Value = Utility.ToNumericControlValue(data["Vaccine"]["MortalityRateMax"]);
            numericUpDownVaccineEfficacyRateMin.Value = Utility.ToNumericControlValue(data["Vaccine"]["EfficacyRateMin"]);
            numericUpDownVaccineEfficacyRateMax.Value = Utility.ToNumericControlValue(data["Vaccine"]["EfficacyRateMax"]);

            if (comboBoxDisease.Focused)
                setStatus($"Loaded '{comboBoxDisease.Text}' from file.");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var (populationConfigCount, diseaseConfigCount) = RefreshComboBoxes(); 

            setStatus($"Found {populationConfigCount} population file{(populationConfigCount != 1 ? "s" : "")} and {diseaseConfigCount} disease file{(diseaseConfigCount != 1 ? "s" : "")}.");

            // Load files on start
            if (populationConfigCount > 0)
                comboBoxPopulation.SelectedIndex = 0;
            if(diseaseConfigCount > 0)
                comboBoxDisease.SelectedIndex = 0;
        }

        private (int, int) RefreshComboBoxes()
        {
            int populationConfigCount = 0, diseaseConfigCount = 0;

            comboBoxPopulation.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(Constants.INIPopulationsPath, "*.ini"))
            {
                comboBoxPopulation.Items.Add(Path.GetFileNameWithoutExtension((file)));
                populationConfigCount++;
            }

            comboBoxDisease.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(Constants.INIDiseasesPath, "*.ini"))
            {
                comboBoxDisease.Items.Add(Path.GetFileNameWithoutExtension((file)));
                diseaseConfigCount++;
            }

            return (populationConfigCount, diseaseConfigCount);
        }

        private void setStatus(String text)
        {
            labelStatus.Text = $"[{DateTime.Now.ToString("hh:mm:ss")}] {text}";
        }

        private Disease getDisease()
        {
            double mortalityRateMin = Convert.ToDouble(numericUpDownDiseaseMortalityRateMin.Text);
            double mortalityRateMax = Convert.ToDouble(numericUpDownDiseaseMortalityRateMax.Text);
            double infectionRateMin = Convert.ToDouble(numericUpDownDiseaseInfectionRateMin.Text);
            double infectionRateMax = Convert.ToDouble(numericUpDownDiseaseInfectionRateMax.Text);
            int incubationPeriodMin = Convert.ToInt32(numericUpDownDiseaseIncubationPeriodMin.Text);
            int incubationPeriodMax = Convert.ToInt32(numericUpDownDiseaseIncubationPeriodMax.Text);
            int latencyPeriodMin = Convert.ToInt32(numericUpDownDiseaseLatencyPeriodMin.Text);
            int latencyPeriodMax = Convert.ToInt32(numericUpDownDiseaseLatencyPeriodMax.Text);
            int terminationPeriodMin = Convert.ToInt32(numericUpDownDiseaseTerminationPeriodMin.Text);
            int terminationPeriodMax = Convert.ToInt32(numericUpDownDiseaseTerminationPeriodMax.Text);

            return new Disease(mortalityRateMin, mortalityRateMax, infectionRateMin, infectionRateMax, incubationPeriodMin, incubationPeriodMax, latencyPeriodMin, latencyPeriodMax, terminationPeriodMin, terminationPeriodMax);
        }
    }
}
