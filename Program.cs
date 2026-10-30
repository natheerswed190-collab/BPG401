
using System;

// Requirement: Display system information and configure console output.
Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintSystemInformation();

// Requirement: Create a random number generator for generating questions.
Random random = new Random();

// Requirement: Ask the user for a positive number of questions.
int questionCount = ReadPositiveInteger(
    "Please enter the maximum number of questions: ");

// Requirement: Read the player's information and validate accepted characters.
string playerInformation = ReadPlayerInformation();

// Requirement: Display the player's information and distinct accepted characters.
Console.WriteLine();
Console.WriteLine("Your full information: " + playerInformation);
Console.WriteLine(
    "Distinct accepted characters: " +
    GetDistinctAcceptedCharacters(playerInformation));
Console.WriteLine();

// Requirement: Store the generated words for every question.
// Each question contains between 4 and 8 words.
string[,] generatedWords = new string[questionCount, 8];
int[] wordCounts = new int[questionCount];

// Requirement: Store the user's answers and the correct answers.
// Each question requires four answers.
string[,] userAnswers = new string[questionCount, 4];
string[,] correctAnswers = new string[questionCount, 4];

// Requirement: Store the evaluation result for each question.
int[] evaluationResults = new int[questionCount];

// Requirement: Define the four questions asked after generating the text.
string[] answerLabels =
{
    "Total number of characters",
    "Words containing vowels",
    "Words containing lowercase letters",
    "Words containing special characters"
};

// Requirement: Initialize counters for correct answers and final statistics.
int[] correctByCategory = new int[4];
int totalCorrectAnswers = 0;
int totalAnswers = questionCount * answerLabels.Length;

// Requirement: Generate a new random text and process each question.
for (int questionIndex = 0;
     questionIndex < questionCount;
     questionIndex++)
{
    // Requirement: Generate between 4 and 8 random words.
    // Every word has 5 to 10 characters and starts with an English letter.
    string[] words = GenerateRandomWords(random);

    // Requirement: Store the generated words in the words array for this question.
    wordCounts[questionIndex] = words.Length;

    for (int i = 0; i < words.Length; i++)
    {
        generatedWords[questionIndex, i] = words[i];
    }

    // Requirement: Combine the generated words into one text and display it.
    string generatedText = string.Join(" ", words);

    Console.WriteLine("==================================================");
    Console.WriteLine("Question " + (questionIndex + 1));
    Console.WriteLine("Generated text: " + generatedText);
    Console.WriteLine();

    // Requirement: Inform the user that Ignore can skip the question
    // before the first answer is entered.
    Console.WriteLine(
        "Enter Ignore to skip this question before answering the first question.");
    Console.WriteLine();

    bool ignored = false;
    int correctInQuestion = 0;

    // Requirement: Calculate and store the correct answers for this question.
    // 1. Calculate the total number of characters in all words.
    correctAnswers[questionIndex, 0] =
        GetTotalCharacterCount(words).ToString();

    // 2. Count words containing at least one vowel.
    correctAnswers[questionIndex, 1] =
        CountWordsWithVowels(words).ToString();

    // 3. Count words containing at least one lowercase letter.
    correctAnswers[questionIndex, 2] =
        CountWordsWithLowercaseLetters(words).ToString();

    // 4. Count words containing at least one special character.
    correctAnswers[questionIndex, 3] =
        CountWordsWithSpecialCharacters(words).ToString();

    // Requirement: Ask the first question before checking for Ignore.
    Console.Write(answerLabels[0] + ": ");
    string firstAnswer = Console.ReadLine() ?? "";

    // Requirement: Recognize Ignore without distinguishing uppercase
    // and lowercase letters.
    if (firstAnswer.Trim().Equals(
        "Ignore",
        StringComparison.OrdinalIgnoreCase))
    {
        ignored = true;

        // Requirement: Store the user's input even when the question is ignored.
        userAnswers[questionIndex, 0] = firstAnswer;

        for (int category = 1;
             category < answerLabels.Length;
             category++)
        {
            userAnswers[questionIndex, category] = "";
        }

        // Requirement: An ignored question receives evaluation result 0.
        evaluationResults[questionIndex] = 0;

        Console.WriteLine("Question ignored and recorded as incorrect.");
    }
    else
    {
        // Requirement: Store the user's first answer.
        userAnswers[questionIndex, 0] = firstAnswer;

        // Requirement: Ask for and store the remaining three answers.
        // Ignore is no longer treated as a skip command after the first answer.
        for (int category = 1;
             category < answerLabels.Length;
             category++)
        {
            Console.Write(answerLabels[category] + ": ");
            userAnswers[questionIndex, category] =
                Console.ReadLine() ?? "";
        }

        // Requirement: Compare each user answer with the corresponding
        // correct answer and count the correct answers.
        for (int category = 0;
             category < answerLabels.Length;
             category++)
        {
            if (NormalizeAnswer(userAnswers[questionIndex, category]) ==
                NormalizeAnswer(correctAnswers[questionIndex, category]))
            {
                correctByCategory[category]++;
                totalCorrectAnswers++;
                correctInQuestion++;
            }
        }

        // Requirement: Store an evaluation result for each question.
        // 0 = all answers are incorrect.
        // 1 = all answers are correct.
        // 2 = at least one answer is correct and at least one is incorrect.
        if (correctInQuestion == 0)
        {
            evaluationResults[questionIndex] = 0;
        }
        else if (correctInQuestion == answerLabels.Length)
        {
            evaluationResults[questionIndex] = 1;
        }
        else
        {
            evaluationResults[questionIndex] = 2;
        }
    }

    // Requirement: Display the correct answers after each question.
    Console.WriteLine();
    Console.WriteLine("Correct answers for this question:");

    for (int category = 0;
         category < answerLabels.Length;
         category++)
    {
        Console.WriteLine(
            "- " + answerLabels[category] + ": " +
            correctAnswers[questionIndex, category]);
    }

    // Requirement: Display the evaluation result stored for this question.
    Console.WriteLine(
        "Evaluation result: " +
        evaluationResults[questionIndex]);

    if (ignored)
    {
        Console.WriteLine("Question status: Ignored (incorrect).");
    }
    else if (evaluationResults[questionIndex] == 0)
    {
        Console.WriteLine("Question status: All answers incorrect.");
    }
    else if (evaluationResults[questionIndex] == 1)
    {
        Console.WriteLine("Question status: All answers correct.");
    }
    else
    {
        Console.WriteLine(
            "Question status: Some answers correct, some incorrect.");
    }

    Console.WriteLine();
}

// Requirement: Display final statistics, category scores,
// question evaluations, and stored generated words.
PrintFinalStatistics(
    answerLabels,
    correctByCategory,
    totalCorrectAnswers,
    totalAnswers,
    evaluationResults,
    generatedWords,
    wordCounts);


// Requirement: Display information about the operating system and computer.
static void PrintSystemInformation()
{
    Console.WriteLine("Windows version: " + Environment.OSVersion);

    Console.WriteLine(
        "64 Bit operating system? : " +
        (Environment.Is64BitOperatingSystem ? "Yes" : "No"));

    Console.WriteLine("PC Name : " + Environment.MachineName);
    Console.WriteLine("Number of CPUS : " + Environment.ProcessorCount);
    Console.WriteLine("Windows folder : " + Environment.SystemDirectory);

    Console.WriteLine(
        "Logical Drives Available : " +
        string.Join(", ", Environment.GetLogicalDrives())
            .TrimEnd(',', ' ')
            .Replace("\\", ""));

    Console.WriteLine();
}


// Requirement: Read an integer and reject invalid values or values <= 0.
static int ReadPositiveInteger(string prompt)
{
    int value;

    Console.Write(prompt);

    while (!int.TryParse(Console.ReadLine(), out value) ||
           value <= 0)
    {
        Console.Write(
            "The number of questions should be an integer > 0. " +
            "Please enter it again: ");
    }

    return value;
}


// Requirement: Read player information containing at least six accepted
// characters from A-Z, a-z, and 0-9.
static string ReadPlayerInformation()
{
    const string acceptedCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    while (true)
    {
        Console.WriteLine(
            "Please enter your name, ID, and any other information on one line.");

        Console.WriteLine(
            "Accepted characters: A-Z, a-z, 0-9. " +
            "At least 6 accepted characters are required.");

        Console.Write("Your information: ");
        string information = Console.ReadLine() ?? "";

        int acceptedCount = 0;

        for (int i = 0; i < information.Length; i++)
        {
            if (acceptedCharacters.IndexOf(information[i]) >= 0)
            {
                acceptedCount++;
            }
        }

        if (acceptedCount >= 6)
        {
            return information;
        }

        Console.WriteLine(
            "The entered text should contain at least 6 accepted characters. Try again.");

        Console.WriteLine();
    }
}


// Requirement: Return each accepted character only once,
// preserving its first occurrence in the player's information.
static string GetDistinctAcceptedCharacters(string information)
{
    const string acceptedCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    string distinctCharacters = "";

    for (int i = 0; i < information.Length; i++)
    {
        char current = information[i];

        if (acceptedCharacters.IndexOf(current) >= 0 &&
            distinctCharacters.IndexOf(current) < 0)
        {
            distinctCharacters += current;
        }
    }

    return distinctCharacters;
}


// Requirement: Randomly generate 4-8 words.
// Each word must contain 5-10 characters.
// The first character must be an uppercase or lowercase English letter.
// The remaining characters may include letters, digits, and # ; ! ?.
static string[] GenerateRandomWords(Random random)
{
    const string letters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    const string allCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#;!?";

    int wordCount = random.Next(4, 9);
    string[] words = new string[wordCount];

    for (int wordIndex = 0;
         wordIndex < wordCount;
         wordIndex++)
    {
        int wordLength = random.Next(5, 11);
        string word = "";

        // Requirement: Ensure that the first character is an English letter.
        word += letters[random.Next(letters.Length)];

        // Requirement: Randomly generate the remaining characters.
        for (int charIndex = 1;
             charIndex < wordLength;
             charIndex++)
        {
            word += allCharacters[
                random.Next(allCharacters.Length)];
        }

        words[wordIndex] = word;
    }

    return words;
}


// Requirement: Calculate the total number of characters
// across all generated words, excluding spaces between words.
static int GetTotalCharacterCount(string[] words)
{
    int total = 0;

    for (int i = 0; i < words.Length; i++)
    {
        total += words[i].Length;
    }

    return total;
}


// Requirement: Count each word once if it contains at least one
// vowel from a, A, e, E, i, I, o, O, u, U, y, Y.
static int CountWordsWithVowels(string[] words)
{
    const string vowels = "aAeEiIoOuUyY";
    int count = 0;

    for (int i = 0; i < words.Length; i++)
    {
        bool containsVowel = false;

        for (int j = 0; j < words[i].Length; j++)
        {
            if (vowels.IndexOf(words[i][j]) >= 0)
            {
                containsVowel = true;
                break;
            }
        }

        if (containsVowel)
        {
            count++;
        }
    }

    return count;
}


// Requirement: Count each word once if it contains
// at least one lowercase English letter from a to z.
static int CountWordsWithLowercaseLetters(string[] words)
{
    int count = 0;

    for (int i = 0; i < words.Length; i++)
    {
        bool containsLowercase = false;

        for (int j = 0; j < words[i].Length; j++)
        {
            if (words[i][j] >= 'a' &&
                words[i][j] <= 'z')
            {
                containsLowercase = true;
                break;
            }
        }

        if (containsLowercase)
        {
            count++;
        }
    }

    return count;
}


// Requirement: Count each word once if it contains
// at least one special character from # ; ! ?.
static int CountWordsWithSpecialCharacters(string[] words)
{
    const string specialCharacters = "#;!?";
    int count = 0;

    for (int i = 0; i < words.Length; i++)
    {
        bool containsSpecialCharacter = false;

        for (int j = 0; j < words[i].Length; j++)
        {
            if (specialCharacters.IndexOf(words[i][j]) >= 0)
            {
                containsSpecialCharacter = true;
                break;
            }
        }

        if (containsSpecialCharacter)
        {
            count++;
        }
    }

    return count;
}


// Requirement: Normalize answers by ignoring surrounding whitespace,
// repeated spaces, tabs, and letter case before comparing answers.
static string NormalizeAnswer(string answer)
{
    string[] parts = answer.Trim()
        .ToLowerInvariant()
        .Split(
            new char[] { ' ', '\t' },
            StringSplitOptions.RemoveEmptyEntries);

    return string.Join(" ", parts);
}


// Requirement: Display final scores, accuracy, results by category,
// each question's evaluation result, and the generated words stored in arrays.
static void PrintFinalStatistics(
    string[] answerLabels,
    int[] correctByCategory,
    int totalCorrectAnswers,
    int totalAnswers,
    int[] evaluationResults,
    string[,] generatedWords,
    int[] wordCounts)
{
    Console.WriteLine("==================================================");
    Console.WriteLine("FINAL RESULTS");

    Console.WriteLine(
        "Correct answers: " +
        totalCorrectAnswers + " / " + totalAnswers);

    Console.WriteLine(
        "Incorrect answers: " +
        (totalAnswers - totalCorrectAnswers) + " / " + totalAnswers);

    double percentage = totalAnswers > 0
        ? (double)totalCorrectAnswers * 100 / totalAnswers
        : 0;

    Console.WriteLine("Accuracy: " + percentage.ToString("F2") + "%");
    Console.WriteLine();
    Console.WriteLine("Results by category:");

    for (int i = 0; i < answerLabels.Length; i++)
    {
        Console.WriteLine(
            "- " + answerLabels[i] + ": " +
            correctByCategory[i] + " / " +
            (totalAnswers / answerLabels.Length));
    }

    // Requirement: Display the evaluation result stored for every question.
    Console.WriteLine();
    Console.WriteLine("Evaluation results for all questions:");

    for (int i = 0; i < evaluationResults.Length; i++)
    {
        Console.WriteLine(
            "Question " + (i + 1) +
            ": Evaluation = " + evaluationResults[i]);
    }

    // Requirement: Display all generated words stored in the array.
    Console.WriteLine();
    Console.WriteLine("Stored generated words:");

    for (int i = 0; i < wordCounts.Length; i++)
    {
        Console.Write("Question " + (i + 1) + ": ");

        for (int j = 0; j < wordCounts[i]; j++)
        {
            if (j > 0)
            {
                Console.Write(" ");
            }

            Console.Write(generatedWords[i, j]);
        }

        Console.WriteLine();
    }
}
